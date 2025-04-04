using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelProgression levelProgression;
    [SerializeField] private GameObject loadScreenPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<GameObject> rewardsPrefab;
    [SerializeField] private List<Transform> spawnList;
    [SerializeField] private int waveQuantity = 3, enemyPerWave = 3;
    private int waveCount, enemyCount;
    private Vector3 lastEnemyPosition;

    void Start()
    {
        if (loadScreenPrefab != null)
        {
            GameObject loadScreen = Instantiate(loadScreenPrefab);
            loadScreen.GetComponent<Animator>().SetTrigger("FadeOut");
            StartCoroutine(DestroyAfterAnimation(loadScreen, loadScreen.GetComponent<Animator>()));
        }
        waveCount = waveQuantity;
        StartWave();
    }

    public void OnEnemyDefeated(GameObject enemy)
    {
        if (enemy != null)
        {
            lastEnemyPosition = enemy.transform.position;
            enemy.GetComponent<EnemyExample>().OnDie -= OnEnemyDefeated;
            enemyCount -= 1;
            WaveCheck();
        }
    }

    private void StartWave()
    {
        if (waveCount > 0)
        {
            enemyCount = enemyPerWave;
            Transform spawnPoint = spawnList[waveQuantity - waveCount];
            float offset = 300f;

            for (int i = 0; i < enemyPerWave; i++)
            {
                Vector3 spawnPosition = spawnPoint.position + new Vector3(i * offset, 0, 0);
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                EnemyExample enemyScript = enemy.GetComponent<EnemyExample>();
                if (enemyScript != null) enemyScript.OnDie += OnEnemyDefeated;
            }
        }
        else SpawnReward();
    }

    private void WaveCheck()
    {
        if (enemyCount <= 0)
        {
            waveCount--;
            StartWave();
        }
    }

    private void SpawnReward()
    {
        if (rewardsPrefab.Count > 0) { Instantiate(rewardsPrefab[Random.Range(0, rewardsPrefab.Count-1)], lastEnemyPosition, Quaternion.identity); }
    }

    IEnumerator DestroyAfterAnimation(GameObject obj, Animator animator)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (stateInfo.length == 0)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        float animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength);

        Destroy(obj);
    }
}