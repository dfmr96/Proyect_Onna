using Player.Stats;
using UnityEngine;

namespace Mutations
{
    [CreateAssetMenu(menuName = "Mutations/Effects/Sangre Maldita")]
    public class GlassCannonEffect : UpgradeEffect
    {
        [SerializeField] private float damageMultiplier = 2f;
        [SerializeField] private float vitalReduction = 0.5f;

        public override void Apply(RuntimeStats player)
        {
            player.Damage *= damageMultiplier;
            player.MaxVitalTime *= 1f - vitalReduction;
        }
    }
}