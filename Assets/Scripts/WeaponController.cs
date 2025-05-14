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
    [SerializeField] private float damage = 10f;
    
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

        //ResetCoolingCooldown();
        FireBullet();
        
        /*if (currentAmmo < 1)
        {
            StartOverheatCooldown();
        }
        else
        {
            StartCoroutine(FireRateCooldown());
            StartCoolingCooldown();
        }*/
    }

    private void FireBullet()
    {
        //if (currentAmmo < 1) return;
        Bullet newBullet = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        newBullet.SetSpeed(bulletSpeed);
        newBullet.SetMaxDistance(bulletMaxDistance);
        //currentAmmo--;
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
        canFire = false;
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    private IEnumerator OverheatCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(overheatCooldown);
        currentAmmo = maxAmmo;
        canFire = true;
    }

    private IEnumerator CoolingCooldown()
    {
        yield return new WaitForSeconds(coolingCooldown);
        currentAmmo = maxAmmo;
        coolingCooldownCoroutine = null;
    }
}