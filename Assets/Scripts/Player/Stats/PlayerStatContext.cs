namespace Player.Stats
{
    public class PlayerStatContext
    {
        private IStatSource source;
        private IStatTarget target;

        private RuntimeStats runtimeStats;
        private MetaStatBlock metaStats;

        public void SetupForRun(StatBlock baseStats, MetaStatBlock metaStats, StatReferences statRefs)
        {
            runtimeStats = new RuntimeStats(baseStats, metaStats, statRefs);
            this.metaStats = metaStats;

            source = runtimeStats;
            target = runtimeStats;

            RunData.SetStats(runtimeStats); // opcional
        }

        public void SetupForHub(IStatSource source, MetaStatBlock metaStats)
        {
            this.metaStats = metaStats;
            this.source = source;
            this.target = metaStats;
        }

        public IStatSource Source => source;
        public IStatTarget Target => target;

        public RuntimeStats Runtime => runtimeStats;
        public MetaStatBlock Meta => metaStats;
    }
}