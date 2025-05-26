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
            player.MovementSpeed *= 1f + increasePercent;
        }
    }
}