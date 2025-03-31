using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private int maxAmmo = 12;
    [SerializeField] private int currentAmmo;
    [SerializeField] private float overheatCooldown = 1.25f;
    [SerializeField] private float coolingCooldown = 2f;
    [SerializeField] private float bulletMaxDistance = 10f;
    
    private bool canFire;
    private Coroutine coolingCooldownCoroutine;

    private void Start()
    {
        canFire = true;
        currentAmmo = maxAmmo;
    }

    public void Attack()
    {
        if (!canFire || currentAmmo < 1) return;

        ResetCoolingCooldown();
        FireBullet();
        
        if (currentAmmo < 1)
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
        if (currentAmmo < 1) return;
        Bullet newBullet = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        newBullet.SetSpeed(bulletSpeed);
        newBullet.SetMaxDistance(bulletMaxDistance);
        currentAmmo--;
    }
    private void StartCoolingCooldown()
    {
        coolingCooldownCoroutine ??= StartCoroutine(CoolingCooldown());
    }

    private void ResetCoolingCooldown()
    {
        if (coolingCooldownCoroutine == null) return;
        StopCoroutine(coolingCooldownCoroutine);
        coolingCooldownCoroutine = null;
    }
    private void StartOverheatCooldown()
    {
        ResetCoolingCooldown();
        StartCoroutine(OverheatCooldown());
    }

    private IEnumerator FireRateCooldown()
    {
        Debug.Log("FireRate cooldown Started");
        canFire = false;
        yield return new WaitForSeconds(fireRate);
        canFire = true;
        Debug.Log("FireRate cooldown Finished");
    }

    private IEnumerator OverheatCooldown()
    {
        Debug.Log("OverheatCooldown Started");
        canFire = false;
        yield return new WaitForSeconds(overheatCooldown);
        currentAmmo = maxAmmo;
        canFire = true;
        Debug.Log("OverheatCooldown Finished");
    }

    private IEnumerator CoolingCooldown()
    {
        Debug.Log("CoolingCooldown Started");
        yield return new WaitForSeconds(coolingCooldown);
        currentAmmo = maxAmmo;
        Debug.Log("CoolingCooldown Finished");
        coolingCooldownCoroutine = null;
    }
}