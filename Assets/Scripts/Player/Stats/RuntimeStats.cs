using UnityEngine;

namespace Player.Stats
{
    [System.Serializable]
    public class RuntimeStats
    {
        [SerializeField] private float currentEnergyTime;
        [SerializeField] private float drainRatePerSecond;
        [SerializeField] private float maxEnergyTime;
        [SerializeField] private float moveSpeed;
        
        public float CurrentEnergyTime => currentEnergyTime;
        public float DrainRatePerSecond => drainRatePerSecond;
        public float MaxEnergyTime => maxEnergyTime;
        public float MoveSpeed => moveSpeed;
        
        public RuntimeStats(CharacterBaseStats baseStats, MetaProgressionStats meta)
        {
            maxEnergyTime = baseStats.MaxEnergyTime + meta.BonusMaxTime;
            // Ensure that the max energy time is not less than the start energy time
            currentEnergyTime = Mathf.Min(baseStats.StartEnergyTime + meta.BonusStartTime, maxEnergyTime);
            drainRatePerSecond = baseStats.DrainRatePerSecond + meta.BonusDrainReduction;
            moveSpeed = baseStats.BaseMoveSpeed + meta.BonusMoveSpeed;
        }
        //TODO Modifier methods to add and remove bonuses
    }                
}
