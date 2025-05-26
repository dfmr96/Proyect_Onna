using UnityEngine;

namespace Player.Stats
{
    [CreateAssetMenu(menuName = "Stats/Stat References", fileName = "StatReferences")]
    public class StatReferences : ScriptableObject
    {
        [Header("Vital Stats")]
        public StatDefinition maxVitalTime;
        public StatDefinition initialVitalTime;
        public StatDefinition passiveDrainRate;
        public StatDefinition enemyHitPenalty;

        [Header("Combat Stats")]
        public StatDefinition damage;
        public StatDefinition attackRange;
        public StatDefinition fireRate;
        public StatDefinition criticalChance;
        public StatDefinition criticalDamageMultiplier;
        public StatDefinition damageResistance;
        public StatDefinition overheatCooldown;

        [Header("Movement Stats")]
        public StatDefinition movementSpeed;
        public StatDefinition dashDistance;
        public StatDefinition dashCooldown;

#if UNITY_EDITOR
        [ContextMenu("Auto Link From Registry")]
        public void AutoLinkFromRegistry()
        {
            var registry = UnityEditor.AssetDatabase.LoadAssetAtPath<StatRegistry>("Assets/Stats/StatRegistry.asset");

            if (!registry)
            {
                Debug.LogWarning("⚠️ No se encontró el StatRegistry en Assets/Stats/StatRegistry.asset");
                return;
            }

            TryAssign(ref maxVitalTime, "MaxVitalTime", registry);
            TryAssign(ref initialVitalTime, "InitialVitalTime", registry);
            TryAssign(ref passiveDrainRate, "PassiveDrainRate", registry);
            TryAssign(ref enemyHitPenalty, "EnemyHitPenalty", registry);
            TryAssign(ref damage, "Damage", registry);
            TryAssign(ref attackRange, "AttackRange", registry);
            TryAssign(ref fireRate, "FireRate", registry);
            TryAssign(ref criticalChance, "CriticalChance", registry);
            TryAssign(ref criticalDamageMultiplier, "CriticalDamageMultiplier", registry);
            TryAssign(ref damageResistance, "DamageResistance", registry);
            TryAssign(ref overheatCooldown, "OverheatCooldown", registry);
            TryAssign(ref movementSpeed, "MovementSpeed", registry);
            TryAssign(ref dashDistance, "DashDistance", registry);
            TryAssign(ref dashCooldown, "DashCooldown", registry);

            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();

            Debug.Log("✅ StatReferences actualizadas desde el StatRegistry.");
        }

        private void TryAssign(ref StatDefinition field, string statName, StatRegistry registry)
        {
            var stat = registry.GetByName(statName);
            if (stat != null)
            {
                field = stat;
            }
            else
            {
                Debug.LogWarning($"⚠️ Stat '{statName}' no encontrada en el StatRegistry.");
            }
        }
#endif
    }
}
