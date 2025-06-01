using NaughtyAttributes;
using Player.Stats;
using UnityEngine;

namespace Mutations
{
    public abstract class UpgradeEffect : ScriptableObject
    {
        [Header("🔬 Test Config (Editor Only)")]
        [SerializeField] protected StatReferences statRefs;
        [SerializeField] protected MetaStatBlock testMetaStats;
        [SerializeField] protected StatBlock testBaseStats;
        public abstract void Apply(RuntimeStats player);
    }
}