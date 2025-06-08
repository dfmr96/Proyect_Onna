using System;
using Core;
using Player.Stats;
using Player.Stats.Meta;
using Player.Stats.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerModelBootstrapper : MonoBehaviour
    {
        [SerializeField] private GameMode currentMode;

        [Header("Stats Setup")]
        [SerializeField] private StatBlock baseStats;
        [SerializeField] private MetaStatBlock metaStats;
        [SerializeField] private StatReferences statRefs;
        [SerializeField] private StatRegistry registry;
        
        private PlayerStatContext _statContext;

        public MetaStatBlock MetaStats => metaStats;

        public StatRegistry Registry => registry;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (!ValidateDependencies()) return;
            
            MetaStatSaveSystem.Load(metaStats, registry);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnEnable()
        {
            Debug.Log("🛰 Bootstrapper suscribiéndose al PlayerSpawnedSignal");
            EventBus.Subscribe<PlayerSpawnedSignal>(OnPlayerSpawned);
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe<PlayerSpawnedSignal>(OnPlayerSpawned);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mod)
        {
            currentMode = scene.name == "HUB" ? GameMode.Hub : GameMode.Run;
            GameModeSelector.SelectedMode = currentMode;
        }
        
        private bool ValidateDependencies()
        {
            bool isValid = true;

            if (metaStats == null)
            {
                Debug.LogError("❌ MetaStats no asignado en el Inspector.");
                isValid = false;
            }

            if (registry == null)
            {
                Debug.LogError("❌ Registry no asignado en el Inspector.");
                isValid = false;
            }

            return isValid;
        }

        private void OnPlayerSpawned(PlayerSpawnedSignal signal)
        {
            Debug.Log("🧠 Bootstrapper: Recibida señal de jugador spawneado");
            var playerGO = signal.PlayerGO;
            var playerModel = playerGO.GetComponent<PlayerModel>();
            if (playerModel == null)
            {
                Debug.LogError("❌ PlayerModel no encontrado en el jugador instanciado.");
                return;
            }

            _statContext = new PlayerStatContext();

            switch (currentMode)
            {
                case GameMode.Run:
                    var runtimeStats = RunData.CurrentStats ?? new RuntimeStats(baseStats, metaStats, statRefs);
                    RunData.SetStats(runtimeStats);
                    _statContext.SetupFromExistingRuntime(runtimeStats, metaStats);
                    break;

                case GameMode.Hub:
                    var reader = new MetaStatReader(baseStats, metaStats);
                    _statContext.SetupForHub(reader, metaStats);
                    metaStats.InjectBaseSource(reader);
                    break;

                default:
                    Debug.LogError("❌ Modo de juego inválido.");
                    return;
            }

            Debug.Log("✅ StatContext inyectado correctamente en PlayerModel.");
            playerModel.InjectStatContext(_statContext);

        }
    }
    
        
    public static class GameModeSelector
    {
        public static GameMode SelectedMode = GameMode.Hub;
    }


    public enum GameMode
    {
        Hub,
        Run
    }
    
    
}