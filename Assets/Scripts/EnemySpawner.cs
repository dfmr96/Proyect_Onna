using UnityEngine;
using System;
using UnityEngine.AI;
using Player;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int wavesQuantity = 3;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Vector3 spawnAreaCenter;
    [SerializeField] private Vector3 spawnAreaSize;
    [SerializeField] private float safeDistanceFromPlayer = 5f;
    private Transform playerTransform;
    private int maxTries = 30;
    public Action OnAllWavesCompleted;
    //public Action OnWaveCompleted;    --  Se puede usar para que pase algo entre oleadas
    private int actualWave = 0;
    private int enemiesQuantity = 0;

    private void Start() 
    {
        playerTransform = PlayerHelper.GetPlayer().transform;
        StarWave();
    }
    public void StarWave()
    {
        actualWave++;
        Vector3 randomPosition;
        if (TryGetRandomNavMeshPosition(out randomPosition))
            transform.position = randomPosition;
        else Debug.LogWarning("No se encontró posición válida sobre el NavMesh.");
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoints[i].position, Quaternion.identity);
            enemy.GetComponent<EnemyModel>().OnDeath += OnEnemyDeath;
            enemiesQuantity++;
        }
    }
    private bool TryGetRandomNavMeshPosition(out Vector3 result)
    {
        for (int i = 0; i < maxTries; i++)
        {
            Vector3 randomPoint = spawnAreaCenter + new Vector3(UnityEngine.Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2), 0, UnityEngine.Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2));
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                if (playerTransform != null)
                {
                    if (Vector3.Distance(hit.position, playerTransform.position) >= safeDistanceFromPlayer)
                    {
                        result = hit.position;
                        return true;
                    }
                }
            }
        }
        result = Vector3.zero;
        return false;
    }
    private void OnEnemyDeath(EnemyModel enemy)
    {
        enemy.OnDeath -= OnEnemyDeath;
        enemiesQuantity--;
        if (enemiesQuantity <= 0)
        {
            if (actualWave <= wavesQuantity)
            {
                StarWave();
                //OnWaveCompleted?.Invoke();
            }
            else OnAllWavesCompleted?.Invoke();
        }
    }
}
