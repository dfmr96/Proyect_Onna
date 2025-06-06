using System.Collections.Generic;
using Player.Stats.Interfaces;
using Player.Stats.Meta;
using UnityEngine;

namespace Player.Stats.Runtime
{
    [System.Serializable]
    public class RuntimeStats : IStatContainer, IStatSource, IStatTarget
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
                 + (MetaStats?.Get(stat) ?? 0f)
                 + runtimeBonuses.Get(stat);
        }

        public void Set(StatDefinition stat, float value)
        {
            runtimeBonuses.Set(stat, value);
        }

        public IReadOnlyDictionary<StatDefinition, float> All => null; // Podés implementar si lo necesitás

        public MetaStatBlock MetaStats => metaStats;

        public void SetCurrentEnergyTime(float value, float maxVitalTime)
        {
            currentEnergyTime = Mathf.Clamp(value, 0f, maxVitalTime);
        }

        public void ClearRuntimeBonuses() => runtimeBonuses.Clear();

        public float GetBaseValue(StatDefinition stat) => baseStats.Get(stat);

        public float GetBonusValue(StatDefinition stat) => runtimeBonuses.Get(stat);
        
        public Dictionary<StatDefinition, float> GetAllRuntimeBonuses()
        {
            return new Dictionary<StatDefinition, float>(runtimeBonuses.All);
        }

        public void AddFlatBonus(StatDefinition stat, float value)
        {
            float existing = runtimeBonuses.Get(stat);
            runtimeBonuses.Set(stat, existing + value);
        }

        public void AddPercentBonus(StatDefinition stat, float percent)
        {
            float basePlusMeta = GetBaseValue(stat) + (MetaStats?.Get(stat) ?? 0f);
            float delta = basePlusMeta * (percent / 100f);
            AddFlatBonus(stat, delta);
        }
        public void AddMultiplierBonus(StatDefinition stat, float factor)
        {
            float baseVal = Get(stat);
            float newValue = baseVal * factor;
            AddFlatBonus(stat, newValue - baseVal);
        }
    }
}
