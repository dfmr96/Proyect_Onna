using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] private LevelProgression levelProgression;
    [SerializeField] private GameObject loadCanvasPrefab;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
            OnTrigger(other);
    }

    protected virtual void OnTrigger(Collider other) { }
    protected virtual void SavePlayerData(Collider other) { RunData.CurrentStats.SetCurrentEnergyTime(other.GetComponent<PlayerModel>().CurrentTime); }
    protected virtual void LoadNextLevel() { SceneManagementUtils.AsyncLoadSceneByName(levelProgression.GetNextRoom(), loadCanvasPrefab, this); }
    protected virtual void LoadLevelByName(string sceneName) { SceneManagementUtils.AsyncLoadSceneByName(sceneName, loadCanvasPrefab, this); }
}
