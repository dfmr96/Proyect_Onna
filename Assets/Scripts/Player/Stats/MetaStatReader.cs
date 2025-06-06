using System;
using Player.Stats;

namespace Player.Stats
{
    [Serializable]
    public class MetaStatReader : IStatSource
    {
        private StatBlock baseStats;
        private MetaStatBlock metaStats;

        public MetaStatReader(StatBlock baseStats, MetaStatBlock metaStats)
        {
            this.baseStats = baseStats;
            this.metaStats = metaStats;
        }

        public float Get(StatDefinition stat)
        {
            return baseStats.Get(stat) + metaStats.Get(stat);
        }
        
        public float GetBase(StatDefinition stat)
        {
            return baseStats.Get(stat);
        }

        public float GetMeta(StatDefinition stat)
        {
            return metaStats.Get(stat);
        }
    }
}