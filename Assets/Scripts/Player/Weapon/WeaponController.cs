using System.Collections;
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

        private void Start()
        {
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
            var bullet = Instantiate(bulletSetting.bulletPrefab, bulletSetting.bulletSpawnPoint.position, bulletSetting.bulletSpawnPoint.rotation);
            bullet.SetSpeed(bulletSetting.bulletSpeed);
            bullet.SetMaxDistance(bulletSetting.bulletMaxDistance);
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
            yield return new WaitForSeconds(cooldownSettings.fireRate);
            canFire = true;
        }

        private IEnumerator OverheatCooldown()
        {
            canFire = false;
            yield return new WaitForSeconds(cooldownSettings.overheatCooldown);
            currentAmmo = ammoSettings.maxAmmo;
            canFire = true;
        }

        private IEnumerator CoolingCooldown()
        {
            yield return new WaitForSeconds(cooldownSettings.coolingCooldown);
            currentAmmo = ammoSettings.maxAmmo;
            _coolingCooldownCoroutine = null;
        }
    }
}
