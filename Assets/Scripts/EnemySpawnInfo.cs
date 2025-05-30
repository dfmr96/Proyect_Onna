using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject prefab;

    [Range(0, 100)]
    public float percentChance;

    [HideInInspector]
    public int weight;
}