using UnityEngine;

namespace Player.Stats
{
    [System.Serializable]
    public class RuntimeStats
    {
        private float currentEnergyTime;

        private StatBlock baseStats;
        private StatBlock runtimeBonuses;

        public float CurrentEnergyTime => currentEnergyTime;

        public RuntimeStats(CharacterBaseStats baseStatsAsset, StatReferences references)
        {
            baseStats = Object.Instantiate(baseStatsAsset.BaseStats);
            runtimeBonuses = ScriptableObject.CreateInstance<StatBlock>();

            float maxVital = baseStats.Get(references.maxVitalTime);
            float start = baseStats.Get(references.initialVitalTime);
            currentEnergyTime = Mathf.Min(start, maxVital);
        }

        public float Get(StatDefinition stat)
        {
            return baseStats.Get(stat) + runtimeBonuses.Get(stat);
        }

        public void AddRuntimeBonus(StatDefinition stat, float amount)
        {
            float existing = runtimeBonuses.Get(stat);
            runtimeBonuses.Set(stat, existing + amount);
        }

        public void MultiplyStat(StatDefinition stat, float factor)
        {
            float baseVal = Get(stat);
            float newValue = baseVal * factor;
            AddRuntimeBonus(stat, newValue - baseVal);
        }

        public void SetCurrentEnergyTime(float value, float maxVitalTime)
        {
            currentEnergyTime = Mathf.Clamp(value, 0f, maxVitalTime);
        }
        
        public void ClearRuntimeBonuses() => runtimeBonuses.Clear();
    }
}
