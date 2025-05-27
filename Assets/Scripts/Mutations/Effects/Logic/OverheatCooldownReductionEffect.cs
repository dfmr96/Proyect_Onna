using NaughtyAttributes;
using Player.Stats;
using UnityEngine;

namespace Mutations
{
    [CreateAssetMenu(menuName = "Mutations/Effects/Tactica de Guerra")]
    public class OverheatCooldownReductionEffect : UpgradeEffect
    {
        [SerializeField] private float reduction = 0.1f;

        public override void Apply(RuntimeStats player)
        {
            float reductionFactor = 1f - (reduction / 100f);
            player.MultiplyStat(statRefs.overheatCooldown, reductionFactor);
        }
        
#if UNITY_EDITOR
        [Button("🔬 Test Effect")]
        private void TestEffect()
        {
            if (statRefs == null || statRefs.overheatCooldown == null || testBaseStats == null)
            {
                Debug.LogWarning("⚠️ Faltan testBaseStats o referencias para testear.");
                return;
            }

            var testStats = new RuntimeStats(testBaseStats, statRefs);
            float before = testStats.Get(statRefs.overheatCooldown);

            Apply(testStats);

            float after = testStats.Get(statRefs.overheatCooldown);
            Debug.Log($"🧪 {name}: OverheatCooldown\nAntes: {before:F2} → Después: {after:F2}");
        }
#endif
    }
}