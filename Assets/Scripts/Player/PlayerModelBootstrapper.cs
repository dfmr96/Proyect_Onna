using System;
using Player.Stats;
using Player.Stats.Meta;
using Player.Stats.Runtime;
using UnityEngine;

namespace Player
{
    public class PlayerModelBootstrapper : MonoBehaviour
    {
        [SerializeField] private PlayerModel playerModel;
        [SerializeField] private GameMode currentMode;

        [Header("Stats Setup")]
        [SerializeField] private StatBlock baseStats;
        [SerializeField] private MetaStatBlock metaStats;
        [SerializeField] private StatReferences statRefs;
        [SerializeField] private StatRegistry registry;

        public MetaStatBlock MetaStats => metaStats;

        public StatRegistry Registry => registry;


        private void Awake()
        {
            if (metaStats == null || registry == null)
            {
                Debug.LogError("❌ MetaStats o Registry no asignados en el Inspector.");
                return;
            }

            MetaStatSaveSystem.Load(metaStats, registry);

            currentMode = GameModeSelector.SelectedMode;
            var statContext = new PlayerStatContext();

            if (currentMode == GameMode.Run)
            {
                RuntimeStats runtimeStats;

                if (RunData.CurrentStats == null)
                {
                    runtimeStats = new RuntimeStats(baseStats, metaStats, statRefs);
                    RunData.SetStats(runtimeStats);
                }
                else
                {
                    runtimeStats = RunData.CurrentStats;
                }

                statContext.SetupFromExistingRuntime(runtimeStats, metaStats);
                playerModel.InjectStatContext(statContext);
            }
            else
            {
                var reader = new MetaStatReader(baseStats, metaStats);
                statContext.SetupForHub(reader, metaStats);
                metaStats.InjectBaseSource(reader);
            }

            playerModel.InjectStatContext(statContext);
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