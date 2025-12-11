using System;
using Mutations;
using Mutations.Core;
using Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NewMutationController
{
    Dictionary<SystemType, NewMutationSystem> systems = new();
    private List<RadiationEffect> effects = new();
    private MutationDB _db;

    public NewMutationController()
    {
        systems[SystemType.Nerve] = new NewMutationSystem { systemType = SystemType.Nerve };
        systems[SystemType.Integumentary] = new NewMutationSystem { systemType = SystemType.Integumentary };
        systems[SystemType.Muscular] = new NewMutationSystem { systemType = SystemType.Muscular };

        //Debug.Log($"🧬 Initialized systems: {string.Join(", ", systems.Keys)}");
        
    }

    public void InitDB()
    {
        _db = Resources.Load<MutationDB>("MutationDB");
        if (_db == null)
        {
            Debug.LogError("❌ MutationDB not found in Resources/MutationDB!");
        }
        else
        {
            //Debug.Log($"✅ MutationDB loaded successfully. Total radiations: {_db.AllRadiations.Count}");
        }
    }

    public void ResetRun()
    {
        Debug.Log("🔄 Resetting run - clearing all slots and effects.");
        foreach (var system in systems.Values)
        {
            system.mayorSlot.Mutation = null;
            system.menorSlot.Mutation = null;
        }
        effects.Clear();
        Debug.Log("✅ All mutations and effects cleared.");
    }

    public List<NewRadiationData> RollRadiations(int count = 3)
    {
        if (_db == null)
        {
            Debug.LogError("⚠️ Cannot roll radiations: MutationDB is null.");
            return null;
        }

        List<NewRadiationData> pool = new List<NewRadiationData>(_db.AllRadiations);
        List<NewRadiationData> result = new List<NewRadiationData>();
        var rng = new System.Random();

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = rng.Next(pool.Count);
            var rad = pool[index];
            result.Add(rad);
            pool.RemoveAt(index);
            Debug.Log($"🎲 Rolled Radiation {i + 1}: {rad.name} ({rad.Type})");
        }

        return result;
    }

    public bool EquipRadiation(MutationType radiation, SystemType system, SlotType slot)
    {
        Debug.Log($"🧪 Attempting to equip radiation: {radiation} → {system}.{slot}");

        if (!systems.TryGetValue(system, out var sys))
        {
            Debug.LogError($"❌ System '{system}' not found!");
            return false;
        }

        NewMutationSlot targetSlot = (slot == SlotType.Major) ? sys.mayorSlot : sys.menorSlot;

        if (!targetSlot.IsEmpty)
        {
            Debug.LogWarning($"⚠️ Slot {system}.{slot} is already occupied by {targetSlot.Mutation?.RadiationType}");
            return false;
        }

        RadiationEffect mutation = _db?.GetMutation(radiation, system, slot);
        if (mutation == null)
        {
            Debug.LogError($"❌ Mutation not found in DB: {radiation} for {system}.{slot}");
            return false;
        }

        sys.AssignMutation(mutation, slot);
        effects.Add(mutation);
        Debug.Log($"✅ Equipped {mutation.RadiationType} to {system}.{slot}. Total active effects: {effects.Count}");

        var player = PlayerHelper.GetPlayer();
        if (player != null)
        {
            mutation.ApplyEffect(player, 1);
            Debug.Log($"💥 Applied effect '{mutation.name}' to player.");
        }
        else
        {
            Debug.LogWarning("⚠️ No player found to apply effect.");
        }

        return true;
    }

    public void DebugPrintStatus()
    {
        Debug.Log("🧩=== MUTATION CONTROLLER STATUS ===");
        foreach (var kv in systems)
        {
            var sys = kv.Value;
            var major = sys.mayorSlot.Mutation != null ? sys.mayorSlot.Mutation.RadiationType.ToString() : "Empty";
            var minor = sys.menorSlot.Mutation != null ? sys.menorSlot.Mutation.RadiationType.ToString() : "Empty";
            Debug.Log($"• {sys.systemType}: Major={major}, Minor={minor}");
        }
        Debug.Log($"Total effects applied: {effects.Count}");
    }

    public void ApplyEffects(GameObject player)
    {
        //Debug.Log($"🔥 Applying {effects.Count} effects to {player.name}");
        foreach (RadiationEffect effect in effects)
        {
            Debug.Log($"➡️ Applying {effect.RadiationType}...");
            effect.ApplyEffect(player, 1);
        }
    }

    public RadiationEffect GetEquippedMutation(SystemType system, SlotType slot)
    {
        if (!systems.TryGetValue(system, out var sys))
        {
            Debug.LogError($"❌ GetEquippedMutation: System {system} not found.");
            return null;
        }

        var m = slot == SlotType.Major ? sys.mayorSlot.Mutation : sys.menorSlot.Mutation;
        if (m == null) Debug.Log($"ℹ️ No mutation equipped in {system}.{slot}");
        return m;
    }
    
    public RadiationEffect GetMutationForSlot(MutationType radiation, SystemType system, SlotType slot)
    {
        if (_db == null)
        {
            Debug.LogError("❌ GetMutationForSlot: MutationDB is null!");
            return null;
        }

        Debug.Log($"🔍 Searching mutation in DB: {radiation} | System={system} | Slot={slot}");
        RadiationEffect mutation = _db.GetMutation(radiation, system, slot);

        if (mutation == null)
        {
            Debug.LogWarning($"⚠️ No mutation found for {radiation} ({system}.{slot}) in DB.");
        }
        else
        {
            Debug.Log($"✅ Mutation found: {mutation.name} ({mutation.RadiationType})");
        }

        return mutation;
    }

    public NewRadiationData GetEquippedRadiationData(SystemType system, SlotType slot)
    {
        if (_db == null)
        {
            Debug.LogError("❌ GetEquippedRadiationData: MutationDB is null!");
            return null;
        }

        if (!systems.TryGetValue(system, out var sys))
        {
            Debug.LogError($"❌ System '{system}' not found in systems dictionary.");
            return null;
        }

        var mutation = slot == SlotType.Major ? sys.mayorSlot.Mutation : sys.menorSlot.Mutation;

        if (mutation == null)
        {
            Debug.Log($"ℹ️ No mutation equipped in {system}.{slot}");
            return null;
        }

        var data = _db.GetRadiationData(mutation.RadiationType);
        if (data == null)
        {
            Debug.LogWarning($"⚠️ No radiation data found for type '{mutation.RadiationType}' in DB.");
        }
        else
        {
            Debug.Log($"✅ Retrieved radiation data: {data.name} for {system}.{slot} ({mutation.RadiationType})");
        }

        return data;
    }

}
