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

        GUILayout.BeginArea(new Rect(10, 10, 300, 200), "Runtime Stats", GUI.skin.window);

        var stats = player.RuntimeStats;
        DrawStat("Speed", statRefs.movementSpeed);
        DrawStat("Damage", statRefs.damage);
        DrawStat("Max Vital", statRefs.maxVitalTime);
        DrawStat("Overheat Cooldown", statRefs.overheatCooldown);
        DrawStat("Attack Range", statRefs.attackRange);
        DrawStat("Damage Resistance", statRefs.damageResistance);

        GUILayout.EndArea();
    }
    
    private void DrawStat(string label, StatDefinition def)
    {
        var stats = player.RuntimeStats;
        if (def == null) return;

        float baseVal = stats.GetBaseValue(def);
        float bonus = stats.GetBonusValue(def);
        float total = baseVal + bonus;

        GUILayout.BeginHorizontal();
        GUILayout.Label($"{label}: {baseVal}", GUILayout.Width(120));

        if (!Mathf.Approximately(bonus, 0f))
        {
            GUI.contentColor = Color.green;
            GUILayout.Label($" + {bonus}", GUILayout.Width(60));
            GUI.contentColor = Color.white;
            GUILayout.Label($" = {total}", GUILayout.Width(60));
        }
        else
        {
            GUILayout.Label($"", GUILayout.Width(120));
        }

        GUILayout.EndHorizontal();
    }
}