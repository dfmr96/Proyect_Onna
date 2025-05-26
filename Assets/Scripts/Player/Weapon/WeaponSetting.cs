using UnityEngine;

namespace Player.Weapon
{
    [System.Serializable]
    public class BulletSettings
    {
        public Bullet bulletPrefab;
        public Transform bulletSpawnPoint;
        public float bulletSpeed = 20f;
        public float bulletMaxDistance = 10f;
        public float damage = 10f;
    }

    [System.Serializable]
    public class CooldownSettings
    {
        public float fireRate = 0.2f;
        public float overheatCooldown = 1.25f;
        public float coolingCooldown = 2f;
    }

    [System.Serializable]
    public class AmmoSettings
    {
        public int maxAmmo = 12;
    }
}