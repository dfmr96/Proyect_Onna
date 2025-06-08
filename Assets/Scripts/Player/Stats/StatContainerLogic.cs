using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Player.Stats.Interfaces;
using UnityEngine;

namespace Player.Stats
{
    [System.Serializable]
    public class StatContainerLogic : IStatContainer
    {
        [SerializeField] private List<StatValue> stats = new();
        [SerializeField] private SerializedDictionary<StatDefinition, float> _lookup = new();

        public float Get(StatDefinition stat) => _lookup.TryGetValue(stat, out var value) ? value : 0f;

        public void Set(StatDefinition stat, float value)
        {
            _lookup[stat] = value;

            var existing = stats.Find(s => s.stat == stat);
            if (existing != null)
                existing.value = value;
            else
                stats.Add(new StatValue { stat = stat, value = value });
        }

        public IReadOnlyDictionary<StatDefinition, float> All => _lookup;

        public void Clear()
        {
            stats.Clear();
            _lookup.Clear();
        }

        public void RebuildLookup()
        {
            Dictionary<StatDefinition, float> oldValues = new(_lookup);

            _lookup.Clear();

            foreach (var statValue in stats) 
            {
                if (statValue.stat != null)
                {
                    if (oldValues.TryGetValue(statValue.stat, out var oldVal))
                        statValue.value = oldVal;

                    _lookup[statValue.stat] = statValue.value;
                }
            }
        }

        public List<StatValue> GetRawList() => stats;
    }
}