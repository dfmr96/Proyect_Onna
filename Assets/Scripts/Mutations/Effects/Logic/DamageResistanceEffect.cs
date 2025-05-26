using Player.Stats;
using UnityEngine;

namespace Mutations
{
    [CreateAssetMenu(menuName = "Mutations/Effects/Blindaje Oseo")]
    public class DamageResistanceEffect : UpgradeEffect
    {
        [SerializeField] private float resistanceBonus = 0.05f;

        public override void Apply(RuntimeStats player)
        {
            player.DamageResistance += resistanceBonus;
        }
    }
}