using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelProgression", menuName = "Game/LevelProgression")]
public class LevelProgression : ScriptableObject
{
    public List<string> region_0;
    public List<string> region_1;
    public List<string> region_2;
    public string region_final;

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
        currentRoom++;
        return GetRandomSceneForCurrentRegion();
    }

    private string GetRandomSceneForCurrentRegion()
    {
        List<string> scenes = GetScenesForCurrentRegion();

        if (scenes != null && scenes.Count > 0)
        {
            string selectedScene = scenes[Random.Range(0, scenes.Count)];
            return selectedScene;
        }
        return null;
    }

    private string GetFinalRegion() { return region_final; }

    private List<string> GetScenesForCurrentRegion()
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
