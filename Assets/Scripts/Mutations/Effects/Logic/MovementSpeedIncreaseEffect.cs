using NaughtyAttributes;
using Player.Stats;
using UnityEngine;

namespace Mutations
{
    [CreateAssetMenu(menuName = "Mutations/Effects/Oxigeno ONNA")]
    public class MovementSpeedIncreaseEffect : UpgradeEffect
    {
        [SerializeField] private float increasePercent = 0.05f;

        public override void Apply(RuntimeStats player)
        {
            player.IncreaseStatByPercent(statRefs.movementSpeed, increasePercent);
        }
#if UNITY_EDITOR    
        [Button("🔬 Test Effect")]
        private void TestEffect()
        {
            var testStats = new RuntimeStats(testBaseStats, statRefs);
            float before = testStats.Get(statRefs.movementSpeed);
            Apply(testStats);
            float after = testStats.Get(statRefs.movementSpeed);
            Debug.Log($"🧪 MovementSpeedIncrease aplicado:\nAntes: {before:F2}\nDespués: {after:F2}");
        }
#endif
    }
}