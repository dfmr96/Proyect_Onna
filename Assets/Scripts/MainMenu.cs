using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject loadCanvasPrefab;

    public void PlayButton(string sceneName) { SceneManagementUtils.AsyncLoadSceneByName(sceneName, loadCanvasPrefab, loadCanvasPrefab.GetComponentInChildren<Slider>(), loadCanvasPrefab.GetComponentInChildren<TextMeshProUGUI>(), this); }

    public void ExitButton() { Application.Quit(); }
}
