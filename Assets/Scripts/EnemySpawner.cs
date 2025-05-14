using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int wavesQuantity = 3;
    [SerializeField] private Transform[] spawnPoints;
    public Action OnAllWavesCompleted;
    //public Action OnWaveCompleted;    --  Se puede usar para que pase algo entre oleadas
    private int actualWave = 0;
    private int enemiesQuantity = 0;

    private void Start() { StarWave(); }

    public void StarWave()
    {
        actualWave++;
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoints[i].position, Quaternion.identity);
            enemy.GetComponent<EnemyModel>().OnDeath += OnEnemyDeath;
            enemiesQuantity++;
        }
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
