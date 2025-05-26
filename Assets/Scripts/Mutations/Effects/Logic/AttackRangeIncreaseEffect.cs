using Player.Stats;
using UnityEngine;

namespace Mutations
{
    [CreateAssetMenu(menuName = "Mutations/Effects/Mirada del Umbral")]
    public class AttackRangeIncreaseEffect : UpgradeEffect
    {
        [SerializeField] private float increasePercent = 0.1f;

        public override void Apply(RuntimeStats player)
        {
            player.AttackRange *= 1f + increasePercent;
        }
    }
}