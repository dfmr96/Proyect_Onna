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

        private RuntimeStats _stats;
        private StatReferences _statReferences;

        public void Init(RuntimeStats stats, StatReferences statReferences)
        {
            _statReferences = statReferences;
            _stats = stats;
        }

        public float AttackRange => _stats.Get(_statReferences.attackRange);
        public float Damage => _stats.Get(_statReferences.damage);

        public Bullet BulletPrefab => bulletPrefab;

        public Transform BulletSpawnPoint => bulletSpawnPoint;

        public float BulletSpeed => _bulletSpeed;
    }

    [System.Serializable]
    public class CooldownSettings
    {
        private RuntimeStats _stats;
        private StatReferences _statReferences;
        public void Init(RuntimeStats stats, StatReferences statReferences)
        {
            _statReferences = statReferences;
            _stats = stats;
        }

        public float FireRate => _stats.Get(_statReferences.fireRate);
        public float OverheatCooldown => _stats.Get(_statReferences.overheatCooldown);
        public float CoolingCooldown => _stats.Get(_statReferences.coolingCooldown);
    }

    [System.Serializable]
    public class AmmoSettings
    {
        public int maxAmmo = 12;
    }
}