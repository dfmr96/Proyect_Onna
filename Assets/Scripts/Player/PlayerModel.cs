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

        public float Speed => RuntimeStats.Get(statRefs.movementSpeed);
        private float DrainRate => RuntimeStats.Get(statRefs.passiveDrainRate);
        public float MaxHealth => RuntimeStats.Get(statRefs.maxVitalTime);
        public float CurrentHealth => CurrentTime;

        private RuntimeStats _runtimeStats;

        private float CurrentTime { get; set; }

        public RuntimeStats RuntimeStats => _runtimeStats;

        private void Awake()
        {
            _runtimeStats = RunData.CurrentStats ?? new RuntimeStats(baseStats, statRefs);
            RunData.SetStats(RuntimeStats);

            CurrentTime = RuntimeStats.CurrentEnergyTime;
        }

        private void Update()
        {
            //ApplyPassiveDrain();
        }

        private void ApplyPassiveDrain()
        {
            float damagePerFrame = DrainRate * Time.deltaTime;
            ApplyDamage(damagePerFrame, false);
        }

        public void TakeDamage(float timeTaken)
        {
            ApplyDamage(timeTaken, true);
        }
        
        public void ApplyDamage(float timeTaken, bool applyResistance)
        {
            float resistance = applyResistance ? Mathf.Clamp01(RuntimeStats.Get(statRefs.damageResistance)) : 0f;
            float effectiveDamage = timeTaken * (1f - resistance);

            CurrentTime -= effectiveDamage;
            ClampEnergy();
            Debug.Log($"🧪 Damage recibido: Base = {timeTaken}, Resistance = {(resistance * 100f)}%, Final = {effectiveDamage}");

            OnUpdateTime?.Invoke(CurrentTime / RuntimeStats.Get(statRefs.maxVitalTime));

            if (CurrentTime <= 0)
                Die();
        }

        public void RecoverTime(float timeRecovered)
        {
            CurrentTime = Mathf.Min(CurrentTime + timeRecovered, RuntimeStats.Get(statRefs.maxVitalTime));
            ClampEnergy();
            OnUpdateTime?.Invoke(CurrentTime / RuntimeStats.Get(statRefs.maxVitalTime));
        }

        private void ClampEnergy()
        {
            RuntimeStats.SetCurrentEnergyTime(CurrentTime, RuntimeStats.Get(statRefs.maxVitalTime));
        }

        public void Die() => OnPlayerDie?.Invoke();
    }
}