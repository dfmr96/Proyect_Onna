using UnityEngine;
using Player.Stats;
using Player;

public class RuntimeStatsDebugger : MonoBehaviour
{
    private PlayerModel player;
    [SerializeField] private StatReferences statRefs;

    private void Start()
    {
        player = FindObjectOfType<PlayerModel>();
    }

    private void OnGUI()
    {
        if (player == null || player.RuntimeStats == null) return;

        GUILayout.BeginArea(new Rect(10, 10, 360, 300), "Runtime Stats", GUI.skin.window);

        var stats = player.RuntimeStats;
        DrawStat("Max Vital", statRefs.maxVitalTime);
        DrawStat("Passive Drain Rate", statRefs.passiveDrainRate);
        DrawStat("Damage Resistance", statRefs.damageResistance);
        DrawStat("Speed", statRefs.movementSpeed);
        DrawStat("Damage", statRefs.damage);
        DrawStat("Overheat Cooldown", statRefs.overheatCooldown);
        DrawStat("Attack Range", statRefs.attackRange);

        GUILayout.EndArea();
    }
    
    private void DrawStat(string label, StatDefinition def)
    {
        var stats = player.RuntimeStats;
        if (def == null) return;

        float baseVal = stats.GetBaseValue(def);
        float metaVal = stats.MetaStats?.Get(def) ?? 0f;
        float runtimeBonus = stats.GetBonusValue(def);
        float total = baseVal + metaVal + runtimeBonus;

        GUILayout.BeginHorizontal();
    
        // Label
        GUI.contentColor = Color.white;
        GUILayout.Label($"{label}:", GUILayout.Width(100));
    
        // Base (blanco)
        GUI.contentColor = Color.white;
        GUILayout.Label($"B:{baseVal:0.##}", GUILayout.Width(50));
    
        // Meta (celeste)
        GUI.contentColor = new Color(0.5f, 0.8f, 1f); // azul claro
        GUILayout.Label($"M:{metaVal:0.##}", GUILayout.Width(50));
    
        // Runtime (verde)
        GUI.contentColor = Color.green;
        GUILayout.Label($"R:{runtimeBonus:0.##}", GUILayout.Width(50));
    
        // Total (cian fuerte)
        GUI.contentColor = Color.cyan;
        GUILayout.Label($"= {total:0.##}", GUILayout.Width(60));

        GUI.contentColor = Color.white;
        GUILayout.EndHorizontal();
    }
}