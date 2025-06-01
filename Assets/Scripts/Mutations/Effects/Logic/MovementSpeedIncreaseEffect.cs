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
            var statDef = statRefs.movementSpeed;
            float fullValue = player.Get(statDef); // ✅ incluye base + meta + runtime
            float bonus = fullValue * (increasePercent / 100f);

            player.AddRuntimeBonus(statDef, bonus);

            Debug.Log($"💨 Oxígeno ONИA aplicado: {increasePercent}% de {fullValue} = {bonus}");
        }
#if UNITY_EDITOR    
        [Button("🔬 Test Effect")]
        private void TestEffect()
        {
            var testStats = new RuntimeStats(testBaseStats, testMetaStats, statRefs);
            float before = testStats.Get(statRefs.movementSpeed);
            Apply(testStats);
            float after = testStats.Get(statRefs.movementSpeed);
            Debug.Log($"🧪 MovementSpeedIncrease aplicado:\nAntes: {before:F2}\nDespués: {after:F2}");
        }
#endif
    }
}