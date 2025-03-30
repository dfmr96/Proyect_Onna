using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagementUtils
{
    public static void LoadSceneByName(string sceneName) { SceneManager.LoadScene(sceneName); }
    public static void LoadSceneByIndex(int sceneIndex) { SceneManager.LoadScene(sceneIndex); }
    public static void LoadActiveScene() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
    public static Scene GetActiveScene() { return SceneManager.GetActiveScene(); }

    public static void AsyncLoadSceneByName(string sceneName, GameObject loadingScreenPrefab,  MonoBehaviour mono)
    {
        GameObject loadingScreen = Object.Instantiate(loadingScreenPrefab);
        mono.StartCoroutine(LazyLoad(sceneName, loadingScreen));
    }

    private static IEnumerator LazyLoad(string _sceneName, GameObject loadingScreen)
    {
        Animator animator = loadingScreen.GetComponent<Animator>();

        if (animator != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            while (stateInfo.length == 0)
            {
                yield return null;
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            }

            yield return new WaitForSeconds(stateInfo.length);
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
