using System.Collections.Generic;

namespace Player.Stats
{
    public interface IStatContainer
    {
        float Get(StatDefinition stat);
        void Set(StatDefinition stat, float value);
        IReadOnlyDictionary<StatDefinition, float> All { get; }
    }
}