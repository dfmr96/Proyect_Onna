﻿using System;
using Core;
using NaughtyAttributes;
using Player.Stats;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerModel : MonoBehaviour, IDamageable, IHealable
    {
        
        public static Action OnPlayerDie;
        public static Action<float> OnUpdateTime;

        [InfoBox("WARNING: PlayerModel now requires StatContext injection at runtime via PlayerModelBootstrapper. " +
                 "Make sure this prefab is not used directly without it.", EInfoBoxType.Warning)]        
        [SerializeField] private bool devMode;
        [SerializeField] private StatReferences statRefs;

        private float _currentTime;
        private PlayerStatContext _statContext;
        
        

        public StatReferences StatRefs => statRefs;
        public float Speed => StatContext.Source.Get(statRefs.movementSpeed);
        private float DrainRate => StatContext.Source.Get(statRefs.passiveDrainRate);
        public float MaxHealth => StatContext.Source.Get(statRefs.maxVitalTime);
        public float CurrentHealth => _currentTime;

        public PlayerStatContext StatContext => _statContext;

        private bool _isInitialized;

        private void Start()
        {
            Debug.LogError("Actualización 8/Junio. Ahora Player Model requiere un PlayerModelBootstrapper en escena. " +
                           "Asegúrate de que este prefab no se use directamente sin él.");
        }
        
        public void InjectStatContext(PlayerStatContext context)
        {
            _statContext = context;
            _currentTime = StatContext.Runtime?.CurrentEnergyTime ?? float.PositiveInfinity;

            EventBus.Publish(new PlayerInitializedSignal(this));
            _isInitialized = true;
        }
        
        public void ForceReinitStats()
        {
            /*var oldBonuses = _runtimeStats?.GetAllRuntimeBonuses(); // Necesitarías exponer esto

            _runtimeStats = new RuntimeStats(baseStats, MetaStats, statRefs);
            RunData.SetStats(_runtimeStats);
            CurrentTime = _runtimeStats.CurrentEnergyTime;

            if (oldBonuses != null)
            {
                foreach (var kvp in oldBonuses)
                    _runtimeStats.AddRuntimeBonus(kvp.Key, kvp.Value);
            }*/
        }


        private void Update()
        {
            if (!_isInitialized) return;
            
            if (!devMode || GameModeSelector.SelectedMode != GameMode.Hub)
            {
                ApplyPassiveDrain();
            }
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
            float resistance = applyResistance ? Mathf.Clamp01(StatContext.Source.Get(statRefs.damageResistance)) : 0f;
            float effectiveDamage = timeTaken * (1f - resistance);

            _currentTime -= effectiveDamage;
            ClampEnergy();
            if (applyResistance)
            {
                Debug.Log($"" +
                          $"🧪 Damage recibido: Base = {timeTaken}, " +
                          $"Resistance = {(resistance * 100f)}%, " +
                          $"Final = {effectiveDamage}");
            }

            OnUpdateTime?.Invoke(_currentTime / StatContext.Source.Get(statRefs.maxVitalTime));

            if (_currentTime <= 0)
                Die();
        }

        public void RecoverTime(float timeRecovered)
        {
            _currentTime = Mathf.Min(_currentTime + timeRecovered, StatContext.Source.Get(statRefs.maxVitalTime));
            ClampEnergy();
            OnUpdateTime?.Invoke(_currentTime / StatContext.Source.Get(statRefs.maxVitalTime));
        }

        private void ClampEnergy()
        {
            if (StatContext.Runtime != null)
                StatContext.Runtime.SetCurrentEnergyTime(_currentTime, MaxHealth);
        }

        public void Die() => OnPlayerDie?.Invoke();
    }
}