using UnityEngine;

namespace Player.Stats
{
    [CreateAssetMenu(menuName = "Stats/Player Base Stats")]
    public class CharacterBaseStats : ScriptableObject
    {
        [Header("Health Stats")]
        [SerializeField] private float startEnergyTime = 60f;
        [SerializeField] private float drainRatePerSecond = 1f;
        [SerializeField] private float maxVitalTime = 90f;
        [SerializeField] private float damageResistance = 0f;

        [Header("Movement Stats ")]
        [SerializeField] private float movementSpeed = 5f;
        
        [Header("Weapon Stats")]
        [SerializeField] private float weaponDamage = 10f;
        [SerializeField] private float weaponRange = 15f;
        [SerializeField] private float weaponFireRate = 0.5f;
        [SerializeField] private float overheatCooldown = 2f;

        public float StartEnergyTime => startEnergyTime;

        public float DrainRatePerSecond => drainRatePerSecond;

        public float MaxVitalTime => maxVitalTime;

        public float MovementSpeed => movementSpeed;

        public float OverheatCooldown => overheatCooldown;

        public float WeaponFireRate => weaponFireRate;

        public float WeaponRange => weaponRange;

        public float WeaponDamage => weaponDamage;

        public float DamageResistance => damageResistance;
    }
}
