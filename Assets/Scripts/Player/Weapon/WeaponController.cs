using System;
using System.Collections;
using Core;
using NaughtyAttributes;
using UnityEngine;

namespace Player.Weapon
{
    public class WeaponController : MonoBehaviour
    {
        [BoxGroup("Bullet")] 
        [SerializeField] private BulletSettings bulletSetting;
        [BoxGroup("Cooldown")] 
        [SerializeField] private CooldownSettings cooldownSettings;
        [BoxGroup("Ammo")] 
        [SerializeField] private AmmoSettings ammoSettings;

        [BoxGroup("Runtime Debug"), ReadOnly]
        [SerializeField] private int currentAmmo;
        [BoxGroup("Runtime Debug"), ReadOnly]
        [SerializeField] private bool canFire = true;

        private Coroutine _coolingCooldownCoroutine;
        private PlayerModel playerModel;

        public CooldownSettings Settings => cooldownSettings;

        private void OnEnable()
        {
            EventBus.Subscribe<PlayerInitializedSignal>(OnPlayerReady);
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe<PlayerInitializedSignal>(OnPlayerReady);
        }
        
        private void OnPlayerReady(PlayerInitializedSignal signal)
        {
            playerModel = signal.Model;

            var stats = playerModel.StatContext.Source;
            var refs = playerModel.StatRefs;
            Settings.Init(stats, refs);
            bulletSetting.Init(stats, refs);

            currentAmmo = ammoSettings.maxAmmo;
        }

        public void Attack()
        {
            if (!canFire || currentAmmo <= 0) return;

            FireBullet();
            currentAmmo--;

            if (currentAmmo <= 0)
            {
                StartOverheatCooldown();
            }
            else
            {
                StartCoroutine(FireRateCooldown());
                StartCoolingCooldown();
            }
        }

        private void FireBullet()
        {
            var bullet = Instantiate(bulletSetting.BulletPrefab, bulletSetting.BulletSpawnPoint.position, bulletSetting.BulletSpawnPoint.rotation);
            bullet.SetSpeed(bulletSetting.BulletSpeed);
            bullet.SetMaxDistance(bulletSetting.AttackRange);
        }

        private void StartCoolingCooldown()
        {
            if (_coolingCooldownCoroutine == null)
            {
                _coolingCooldownCoroutine = StartCoroutine(CoolingCooldown());
            }
        }

        private void ResetCoolingCooldown()
        {
            if (_coolingCooldownCoroutine != null)
            {
                StopCoroutine(_coolingCooldownCoroutine);
                _coolingCooldownCoroutine = null;
            }
        }

        private void StartOverheatCooldown()
        {
            ResetCoolingCooldown();
            StartCoroutine(OverheatCooldown());
        }

        private IEnumerator FireRateCooldown()
        {
            canFire = false;
            yield return new WaitForSeconds(Settings.FireRate);
            canFire = true;
        }

        private IEnumerator OverheatCooldown()
        {
            canFire = false;
            yield return new WaitForSeconds(Settings.OverheatCooldown);
            currentAmmo = ammoSettings.maxAmmo;
            canFire = true;
        }

        private IEnumerator CoolingCooldown()
        {
            yield return new WaitForSeconds(Settings.CoolingCooldown);
            currentAmmo = ammoSettings.maxAmmo;
            _coolingCooldownCoroutine = null;
        }
    }
}
