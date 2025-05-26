using UnityEngine;

namespace Player.Stats
{
    [System.Serializable]
    public class RuntimeStats
    {
        [SerializeField] private float currentEnergyTime;
        [SerializeField] private float drainRatePerSecond;
        [SerializeField] private float maxVitalTime;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float overheatCooldown;
        [SerializeField] private float attackRange;
        [SerializeField] private float damage;
        [SerializeField] private float damageResistance;
        
        public float CurrentEnergyTime => currentEnergyTime;
        public float DrainRatePerSecond => drainRatePerSecond;
        public float MaxVitalTime
        {
            get => maxVitalTime;
            set => maxVitalTime = value;
        }

        public float MovementSpeed
        {
            get => movementSpeed;
            set => movementSpeed = value;
        }

        public float OverheatCooldown
        {
            get => overheatCooldown;
            set => overheatCooldown = value;
        }

        public float AttackRange
        {
            get => attackRange;
            set => attackRange = value;
        }

        public float Damage
        {
            get => damage;
            set => damage = value;
        }

        public float DamageResistance
        {
            get => damageResistance;
            set => damageResistance = value;
        }

        public RuntimeStats(CharacterBaseStats baseStats)
        {
            MaxVitalTime = baseStats.MaxVitalTime;
            // Ensure that the max energy time is not less than the start energy time
            currentEnergyTime = Mathf.Min(baseStats.StartEnergyTime, MaxVitalTime);
            drainRatePerSecond = baseStats.DrainRatePerSecond;
            MovementSpeed = baseStats.MovementSpeed;
        }
        //TODO Modifier methods to add and remove bonuses
        public void SetCurrentEnergyTime(float value) { currentEnergyTime = Mathf.Clamp(value, 0f, MaxVitalTime); }
    }                
}
