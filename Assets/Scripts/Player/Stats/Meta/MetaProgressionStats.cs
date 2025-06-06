using UnityEngine;

namespace Player.Stats.Meta
{
    [System.Serializable]
    public class MetaProgressionStats
    {
        [Header("Health Stats")]
        [SerializeField] private float bonusStartTime = 0f;
        [SerializeField] private float bonusDrainReduction = 0f;
        [SerializeField] private float bonusMaxTime = 0f;
        
        [Header("Movement Stats")]
        [SerializeField] private float bonusMoveSpeed = 0f;
        
        public float BonusStartTime => bonusStartTime;

        public float BonusDrainReduction => bonusDrainReduction;

        public float BonusMaxTime => bonusMaxTime;

        public float BonusMoveSpeed => bonusMoveSpeed;
        
        //TODO Initialize values
        //TODO Add methods to add and remove bonuses
    }
}
