using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "LevelProgression", menuName = "Game/LevelProgression")]
public class LevelProgression : ScriptableObject
{
    public List<SceneAsset> region_0;
    public List<SceneAsset> region_1;
    public List<SceneAsset> region_2;
    public SceneAsset region_final;

    public int minRoomsPerLevel = 2;
    public int maxRoomsPerLevel = 3;

    private int currentRegion = 0;
    private int currentRoom = 0;
    private int roomsToPlay = 3;

    public string GetNextRoom()
    {
        if (currentRoom >= roomsToPlay)
        {
            currentRoom = 0;
            currentRegion += 1;
            roomsToPlay = Random.Range(minRoomsPerLevel, maxRoomsPerLevel + 1);

            if (currentRegion > 2)
            {
                ResetProgress();
                return GetFinalRegion();
            }
        }
        currentRoom += 1;
        return GetRandomSceneForCurrentRegion();
    }

    private string GetRandomSceneForCurrentRegion()
    {
        List<SceneAsset> scenes = GetScenesForCurrentRegion();

        if (scenes != null && scenes.Count > 0)
        {
            SceneAsset selectedScene = scenes[Random.Range(0, scenes.Count)];
            return selectedScene.name;
        }
        return null;
    }

    private string GetFinalRegion() { return region_final.name; }

    private List<SceneAsset> GetScenesForCurrentRegion()
    {
        switch (currentRegion)
        {
            case 0: return region_0;
            case 1: return region_1;
            case 2: return region_2;
            default: return null;
        }
    }

    public void ResetProgress()
    {
        currentRegion = 0;
        currentRoom = 0;
        roomsToPlay = Random.Range(minRoomsPerLevel, maxRoomsPerLevel + 1);
    }
}
