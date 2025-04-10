using UnityEngine;

namespace Player.Stats
{
    [CreateAssetMenu(menuName = "Stats/Player Base Stats")]
    public class CharacterBaseStats : ScriptableObject
    {
        [Header("Health Stats")]
        [SerializeField] private float startEnergyTime = 60f;
        [SerializeField] private float drainRatePerSecond = 1f;
        [SerializeField] private float maxEnergyTime = 90f;

        [Header("Movement Stats ")]
        [SerializeField] private float baseMoveSpeed = 5f;

        public float StartEnergyTime => startEnergyTime;

        public float DrainRatePerSecond => drainRatePerSecond;

        public float MaxEnergyTime => maxEnergyTime;

        public float BaseMoveSpeed => baseMoveSpeed;
    }
}
