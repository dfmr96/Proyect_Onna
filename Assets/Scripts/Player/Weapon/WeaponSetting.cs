using Player.Stats;
using UnityEngine;

namespace Player.Weapon
{
    [System.Serializable]
    public class BulletSettings
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform bulletSpawnPoint;
        private float _bulletSpeed = 20f;
        private float _bulletBulletMaxDistance = 10f;
        private StatDefinition _damage;

        private RuntimeStats _stats;

        public void Init(RuntimeStats stats)
        {
            _stats = stats;
        }

        public float BulletMaxDistance => _bulletBulletMaxDistance;
        public float Damage => _stats.Get(_damage);

        public Bullet BulletPrefab => bulletPrefab;

        public Transform BulletSpawnPoint => bulletSpawnPoint;

        public float BulletSpeed => _bulletSpeed;
    }

    [System.Serializable]
    public class CooldownSettings
    {
        private float _fireRate;
        private StatDefinition _overheatCooldown;
        private StatDefinition _coolingCooldown;

        private RuntimeStats _stats;

        public void Init(RuntimeStats stats)
        {
            _stats = stats;
        }

        public float FireRate => _fireRate;
        public float OverheatCooldown => _stats.Get(_overheatCooldown);
        public float CoolingCooldown => _stats.Get(_coolingCooldown);
    }

    [System.Serializable]
    public class AmmoSettings
    {
        public int maxAmmo = 12;
    }
}