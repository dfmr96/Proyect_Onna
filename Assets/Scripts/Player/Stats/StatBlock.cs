using System.Collections.Generic;
using UnityEngine;

namespace Player.Stats
{
    [CreateAssetMenu(menuName = "Stats/Block", fileName = "New StatBlock")]
    public class StatBlock : ScriptableObject
    {
        [SerializeField] private List<StatValue> stats = new();

        private Dictionary<StatDefinition, float> _lookup;

        private void BuildLookup()
        {
            _lookup = new Dictionary<StatDefinition, float>();
            foreach (var stat in stats)
            {
                if (stat.stat != null)
                    _lookup[stat.stat] = stat.value;
            }
        }

        public float Get(StatDefinition stat)
        {
            if (_lookup == null)
                BuildLookup();

            return _lookup.TryGetValue(stat, out float val) ? val : 0f;
        }

        public void Set(StatDefinition stat, float value)
        {
            if (_lookup == null)
                BuildLookup();

            _lookup[stat] = value;

            var serialized = stats.Find(s => s.stat == stat);
            if (serialized != null)
                serialized.value = value;
            else
                stats.Add(new StatValue { stat = stat, value = value });
        }

        public void Clear()
        {
            _lookup?.Clear();
            _lookup = null;
        }

        public IReadOnlyList<StatValue> AllStats => stats;
    }
}
