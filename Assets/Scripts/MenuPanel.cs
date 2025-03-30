using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] GameObject loadCanvasPrefab;

    public void PlayButton(string sceneName) { SceneManagementUtils.AsyncLoadSceneByName(sceneName, loadCanvasPrefab, this); }

    public void OptionsButton() 
    {
        
    }
    public void ExitButton() { Application.Quit(); }
}
