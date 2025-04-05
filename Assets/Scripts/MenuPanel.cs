using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private LevelProgression levelProgression;
    [SerializeField] GameObject loadCanvasPrefab;

    public void PlayButton() 
    {
        levelProgression.ResetProgress();
        SceneManagementUtils.AsyncLoadSceneByName(levelProgression.GetNextRoom(), loadCanvasPrefab, this);
    }

    public void ExitButton() { Application.Quit(); }
}
