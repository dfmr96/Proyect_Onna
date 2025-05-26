using System;
using Player.Stats;
using UnityEngine;

namespace Player
{
    public class PlayerModel : MonoBehaviour, IDamageable, IHealable
    {
        public static Action OnPlayerDie;
        public static Action<float> OnUpdateTime;

        [Header("Stats Config")] 
        [SerializeField] private StatBlock baseStats;
        [SerializeField] private StatReferences statRefs;

        public float Speed => _stats.Get(statRefs.movementSpeed);
        private float DrainRate => _stats.Get(statRefs.passiveDrainRate);
        public float MaxHealth => _stats.Get(statRefs.maxVitalTime);
        public float CurrentHealth => CurrentTime;

        [SerializeField] RuntimeStats _stats;

        private float CurrentTime { get; set; }

        private void Awake()
        {
            _stats = RunData.CurrentStats ?? new RuntimeStats(baseStats, statRefs);
            RunData.SetStats(_stats);

            CurrentTime = _stats.CurrentEnergyTime;
        }

        private void Update()
        {
            ApplyPassiveDrain();
        }

        private void ApplyPassiveDrain()
        {
            float damagePerFrame = DrainRate * Time.deltaTime;
            TakeDamage(damagePerFrame);
        }

        public void TakeDamage(float timeTaken)
        {
            CurrentTime -= timeTaken;
            ClampEnergy();
            OnUpdateTime?.Invoke(CurrentTime / _stats.Get(statRefs.maxVitalTime));

            if (CurrentTime <= 0)
                Die();
        }

        public void RecoverTime(float timeRecovered)
        {
            CurrentTime = Mathf.Min(CurrentTime + timeRecovered, _stats.Get(statRefs.maxVitalTime));
            ClampEnergy();
            OnUpdateTime?.Invoke(CurrentTime / _stats.Get(statRefs.maxVitalTime));
        }

        private void ClampEnergy()
        {
            _stats.SetCurrentEnergyTime(CurrentTime, _stats.Get(statRefs.maxVitalTime));
        }

        public void Die() => OnPlayerDie?.Invoke();
    }
}