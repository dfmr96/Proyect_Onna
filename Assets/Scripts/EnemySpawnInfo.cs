using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject prefab;
    [Range(0f, 1f)]
    public float probability = 0.33f;
}
