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
            player.OverheatCooldown *= 1f - reduction;
        }
    }
}