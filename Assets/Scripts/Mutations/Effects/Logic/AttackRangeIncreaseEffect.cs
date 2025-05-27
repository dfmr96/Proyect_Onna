using NaughtyAttributes;
using Player.Stats;
using UnityEngine;

namespace Mutations
{
    [CreateAssetMenu(menuName = "Mutations/Effects/Mirada del Umbral")]
    public class AttackRangeIncreaseEffect : UpgradeEffect
    {
        [SerializeField] private float increasePercent;
        

        public override void Apply(RuntimeStats player)
        {
            player.IncreaseStatByPercent(statRefs.attackRange, increasePercent);
        }
        
#if UNITY_EDITOR
        [Button("🔬 Test Effect (Editor)")]
        private void TestEffect()
        {
            var stats = new RuntimeStats(testBaseStats, statRefs);

            float before = stats.Get(statRefs.attackRange);

            Apply(stats);

            float after = stats.Get(statRefs.attackRange);

            Debug.Log($"🧪 Stat '{statRefs.attackRange.name}' aplicado:\nAntes: {before:F2}\nDespués: {after:F2}");
        }
#endif
    }
}