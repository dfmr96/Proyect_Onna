using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private LevelProgression levelProgression;
    [SerializeField] GameObject loadCanvasPrefab;

    public void PlayButton() 
    {
        //SceneManagementUtils.AsyncLoadSceneByName(levelProgression.GetNextRoom(), loadCanvasPrefab, this);
        Debug.Log(levelProgression.GetNextRoom());
    }

    public void ExitButton() { Application.Quit(); }
}
