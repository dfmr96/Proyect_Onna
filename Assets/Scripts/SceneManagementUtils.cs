using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagementUtils
{
    public static void LoadSceneByName(string sceneName) { SceneManager.LoadScene(sceneName); }
    public static void LoadSceneByIndex(int sceneIndex) { SceneManager.LoadScene(sceneIndex); }
    public static void LoadActiveScene() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
    public static Scene GetActiveScene() { return SceneManager.GetActiveScene(); }

    public static void AsyncLoadSceneByName(string sceneName, GameObject loadingScreenPrefab, MonoBehaviour mono)
    {
        mono.StartCoroutine(LazyLoad(sceneName, loadingScreenPrefab, loadingScreenPrefab.GetComponent<LoadingScreen>()));
    }

    private static IEnumerator LazyLoad(string sceneName, GameObject loadingScreenPrefab, MonoBehaviour mono)
    {
        GameObject loadingScreen = Object.Instantiate(loadingScreenPrefab);
        Object.DontDestroyOnLoad(loadingScreen);

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

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            loadingScreen.GetComponent<Animator>().SetTrigger("FadeOut");
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}