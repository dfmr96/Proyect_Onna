using Player.Stats;
using UnityEngine;

namespace Mutations
{
    [CreateAssetMenu(menuName = "Mutations/Effects/Segunda Alma")]
    public class MaxVitalTimeIncreaseEffect : UpgradeEffect
    {
        [SerializeField] private float increasePercent = 0.2f;

        public override void Apply(RuntimeStats player)
        {
            player.MaxVitalTime *= 1f + increasePercent;
        }
    }
}