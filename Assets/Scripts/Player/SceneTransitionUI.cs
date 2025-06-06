using Player;
using Player.Stats;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionUI : MonoBehaviour
{
    public void StartRun()
    {
        var bootstrapper = FindObjectOfType<PlayerModelBootstrapper>();
        var metaStats = bootstrapper.MetaStats;
        var registry = bootstrapper.Registry; 
        MetaStatSaveSystem.Save(metaStats, registry);
        GameModeSelector.SelectedMode = GameMode.Run;
        SceneManager.LoadScene("RoomScene");
    }
    
    public void ReturnToHub()
    {
        GameModeSelector.SelectedMode = GameMode.Hub;
        SceneManager.LoadScene("HubTestScene");
    }
    
    
}