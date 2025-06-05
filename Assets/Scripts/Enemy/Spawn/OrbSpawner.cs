using UnityEngine;
using Enemy.Spawn;

public class OrbSpawner : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private RastroOrb rastroOrbPrefab;
    [SerializeField] private int initialPoolSize = 10;
    [SerializeField] private Transform poolParent;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnBehindDistance = 1.5f;
    [SerializeField] private float spawnHeightOffset = 0.5f;

    private ObjectPool<RastroOrb> orbPool;

    private void Awake()
    {
        orbPool = new ObjectPool<RastroOrb>(rastroOrbPrefab, initialPoolSize, poolParent);
    }

    public void SpawnHealingOrb(Vector3 enemyPosition, Vector3 enemyForward)
    {
        RastroOrb orb = orbPool.Get();
        if (orb == null)
        {
            Debug.LogWarning("[OrbSpawner] No orbs available in pool.");
            return;
        }

        Vector3 spawnPos = enemyPosition - enemyForward * spawnBehindDistance + Vector3.up * spawnHeightOffset;
        orb.transform.position = spawnPos;

        orb.Init(() => orbPool.Release(orb));
    }


}
