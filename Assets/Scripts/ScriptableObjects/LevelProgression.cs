using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "LevelProgression", menuName = "Game/LevelProgression")]
public class LevelProgression : ScriptableObject
{
    public List<SceneAsset> region_0 = new List<SceneAsset>();
    public List<SceneAsset> region_1 = new List<SceneAsset>();
    public List<SceneAsset> region_2 = new List<SceneAsset>();
    public List<SceneAsset> region_3 = new List<SceneAsset>();

    public int minRoomsPerLevel = 2;
    public int maxRoomsPerLevel = 3;

    private int currentRegion = 0;
    private int currentRoom = 0;
    private int roomsToPlay = 3;

    public string GetNextRoom()
    {
        if (currentRoom >= roomsToPlay)
        {
            Debug.Log("Entre a GetNextRoom()");
            currentRoom = 0;
            currentRegion += 1;
            roomsToPlay = Random.Range(minRoomsPerLevel, maxRoomsPerLevel + 1);

            if (currentRegion > 3)
            {
                //Aca deberia ir lo que pase cuando termina el juego, por ejemplo cargar un nivel unico donde haya un boss o algo asi
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

    private List<SceneAsset> GetScenesForCurrentRegion()
    {
        switch (currentRegion)
        {
            case 0: return region_0;
            case 1: return region_1;
            case 2: return region_2;
            case 3: return region_3;
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
