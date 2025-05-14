using UnityEngine;

public class HubManager : MonoBehaviour
{
    [SerializeField] private LevelProgression levelProgression;
    [SerializeField] private PlayerSpawner spawner;
    private void Awake()
    {
        levelProgression.ResetProgress();
        RunData.Clear();
        spawner.SpawnPlayer();
    }
}
