using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public static class SceneManagementUtils
{
    public static void LoadSceneByName(string sceneName) { SceneManager.LoadScene(sceneName); }
    public static void LoadSceneByIndex(int sceneIndex) { SceneManager.LoadScene(sceneIndex); }
    public static void LoadActiveScene() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }

    public static void AsyncLoadSceneByName(string sceneName, GameObject loadingScreenPrefab, Slider slider, TextMeshProUGUI text, MonoBehaviour mono)
    {
        GameObject loadingScreen = Object.Instantiate(loadingScreenPrefab);
        mono.StartCoroutine(LazyLoad(sceneName, slider, text));
    }

    private static IEnumerator LazyLoad(string _sceneName, Slider _slider, TextMeshProUGUI _text)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneName);
        while (asyncLoad.isDone == false) 
        {
            float progress = Mathf.Clamp01(asyncLoad.progress/.09f);
            _slider.value = progress;
            _text.text = progress * 100 + "%";
            yield return null;
        }
    }
}
