using System.Collections.Generic;
using UnityEngine;

namespace Player.Stats
{
    [System.Serializable]
    public class RuntimeStats : IStatContainer
    {
        [SerializeField] private StatBlock baseStats;
        [SerializeField] private MetaStatBlock metaStats;
        [SerializeField] private StatContainerLogic runtimeBonuses = new();

        public float CurrentEnergyTime => currentEnergyTime;
        private float currentEnergyTime;

        public RuntimeStats(StatBlock baseStats, MetaStatBlock metaStats, StatReferences references)
        {
            this.metaStats = metaStats ?? new MetaStatBlock();
            Init(baseStats, references);
        }

        public void Init(StatBlock baseStats, StatReferences references)
        {
            this.baseStats = Object.Instantiate(baseStats);
            runtimeBonuses.Clear();

            float maxVital = this.baseStats.Get(references.maxVitalTime);
            float start = this.baseStats.Get(references.initialVitalTime);
            currentEnergyTime = Mathf.Min(start, maxVital);
        }

        public float Get(StatDefinition stat)
        {
            return baseStats.Get(stat)
                 + (metaStats?.Get(stat) ?? 0f)
                 + runtimeBonuses.Get(stat);
        }

        public void Set(StatDefinition stat, float value)
        {
            runtimeBonuses.Set(stat, value);
        }

        public IReadOnlyDictionary<StatDefinition, float> All => null; // Podés implementar si lo necesitás

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

        public float GetBaseValue(StatDefinition stat) => baseStats.Get(stat);

        public float GetBonusValue(StatDefinition stat) => runtimeBonuses.Get(stat);
    }
}
