using NaughtyAttributes;
using UnityEngine;

namespace Player.Stats
{
    [System.Serializable]
    public class RuntimeStats
    {

        [SerializeField] private StatBlock baseStats;
        [SerializeField] private StatBlock runtimeBonuses;
        public float CurrentEnergyTime => currentEnergyTime;

        private float currentEnergyTime;

        public RuntimeStats(StatBlock baseStats, StatReferences references)
        {
            this.baseStats = Object.Instantiate(baseStats);
            runtimeBonuses = ScriptableObject.CreateInstance<StatBlock>();

            float maxVital = this.baseStats.Get(references.maxVitalTime);
            float start = this.baseStats.Get(references.initialVitalTime);
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
        
        public void IncreaseStatByPercent(StatDefinition stat, float percent)
        {
            float baseVal = Get(stat);
            float delta = baseVal * (percent / 100f);
            AddRuntimeBonus(stat, delta);
        }


        public void SetCurrentEnergyTime(float value, float maxVitalTime)
        {
            currentEnergyTime = Mathf.Clamp(value, 0f, maxVitalTime);
        }
        
        public void ClearRuntimeBonuses() => runtimeBonuses.Clear();
    }
}
