using System.Collections;
using Player;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class SceneManagementUtils
{
    private static RoomScenesData _roomData;

    public static void LoadSceneByName(string sceneName) => SceneManager.LoadScene(sceneName);
    public static void LoadSceneByIndex(int sceneIndex) => SceneManager.LoadScene(sceneIndex);
    public static void LoadActiveScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public static Scene GetActiveScene() => SceneManager.GetActiveScene();

    public static void AsyncLoadSceneByName(string sceneName, GameObject loadingScreenPrefab, MonoBehaviour mono)
    {
        // FIX: ahora pasamos bien el 3er parámetro (mono)
        mono.StartCoroutine(LazyLoad(sceneName, loadingScreenPrefab, mono));
    }


    private static IEnumerator LazyLoad(string sceneName, GameObject loadingScreenPrefab, MonoBehaviour mono)
    {
        if (_roomData == null)
            _roomData = LoadDatabase();

        // Crear pantalla de carga
        GameObject loadingScreen = Object.Instantiate(loadingScreenPrefab);
        Object.DontDestroyOnLoad(loadingScreen);

        Animator animator = loadingScreen.GetComponent<Animator>();

        // ---------------- FADE IN ----------------
        if (animator != null)
        {
            animator.SetTrigger("FadeIn");
            yield return WaitForAnim(animator);
        }

        // Preparar callback de escena cargada
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            RoomInfo roomInfo = _roomData.GetRoom(scene.name);
            loadingScreen.GetComponent<LoadingScreen>().SetLevelInfo(roomInfo.Zone, roomInfo.Level);

            // Disparar FadeOut acá también
            if (animator != null)
                animator.SetTrigger("FadeOut");
        }
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Detectar modo de juego
        GameModeSelector.SelectedMode =
            (sceneName == "HUB" || sceneName == "HUB_Tutorial") ? GameMode.Hub : GameMode.Run;

        // Cargar escena
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
            yield return null;

        // ---------------- FADE OUT ----------------
        if (animator != null)
        {
            yield return WaitForAnim(animator);
        }

        // Seguridad TOTAL
        Object.Destroy(loadingScreen);
    }


    private static IEnumerator WaitForAnim(Animator anim)
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        while (info.length == 0)
        {
            yield return null;
            info = anim.GetCurrentAnimatorStateInfo(0);
        }

        // No depende del Timescale funciona incluso con timeScale = 0
        yield return new WaitForSecondsRealtime(info.length);
    }


    private static RoomScenesData LoadDatabase()
    {
        var db = Resources.Load<RoomScenesData>("RoomsDB");
        if (db == null)
            Debug.LogError("No se encontró RoomDatabase en Resources.");

        return db;
    }
}





/*
public static class SceneManagementUtils
{
    private static RoomScenesData _roomData;
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
        if (_roomData == null) _roomData = LoadDatabase();
        
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
            RoomInfo roomInfo = _roomData.GetRoom(scene.name);
            Debug.Log($"{scene.name} loaded with Zone: {roomInfo.Zone}, Subzone: {roomInfo.Level}");
            SceneManager.sceneLoaded -= OnSceneLoaded;
            loadingScreen.GetComponent<LoadingScreen>().SetLevelInfo(roomInfo.Zone, roomInfo.Level);
            loadingScreen.GetComponent<Animator>().SetTrigger("FadeOut");
        }
        SceneManager.sceneLoaded += OnSceneLoaded;

        var currentMode = sceneName == "HUB" || sceneName == "HUB_Tutorial" ? GameMode.Hub : GameMode.Run;
        //No fui yo, fue simon
        GameModeSelector.SelectedMode = currentMode;
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private static RoomScenesData LoadDatabase()
    {
        var db = Resources.Load<RoomScenesData>("RoomsDB");
        if (db == null) Debug.LogError("No se encontró RoomDatabase en Resources.");
        return db;
    }
}

*/