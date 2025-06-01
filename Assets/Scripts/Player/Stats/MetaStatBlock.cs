using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Stats
{
    [Serializable]
    public class MetaStatBlock : IStatContainer
    {
        [SerializeField] private StatContainerLogic container = new();

        public float Get(StatDefinition stat) => container.Get(stat);
        public void Set(StatDefinition stat, float value) => container.Set(stat, value);
        public IReadOnlyDictionary<StatDefinition, float> All => container.All;
        public void Clear() => container.Clear();
        public void RebuildLookup() => container.RebuildLookup();

        public Dictionary<string, float> ToSerializableDict()
        {
            var dict = new Dictionary<string, float>();
            foreach (var entry in container.GetRawList())
                if (entry.stat != null)
                    dict[entry.stat.name] = entry.value;
            return dict;
        }

        public void LoadFromSerializableDict(Dictionary<string, float> data, StatRegistry registry)
        {
            container.Clear();
            foreach (var pair in data)
            {
                var def = registry.GetByName(pair.Key);
                if (def != null)
                    container.Set(def, pair.Value);
            }

            container.RebuildLookup();
        }
    }
}