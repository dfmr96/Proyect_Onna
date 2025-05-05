using UnityEngine;

namespace Player.Stats
{
    public class StatsTest : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private CharacterBaseStats baseStats;
        [SerializeField] private PlayerProgress testProgress;
        [SerializeField] private RuntimeStats runtimeStats;

        [Header("Scriptable Preview")]
        [SerializeField] private float previewStartTime;
        [SerializeField] private float previewDrainRate;
        [SerializeField] private float previewMaxTime;
        [SerializeField] private float previewMoveSpeed;

        //Allow to see the SO values in the inspector
        private void OnValidate()
        {
            if (baseStats != null)
            {
                previewStartTime = baseStats.StartEnergyTime;
                previewDrainRate = baseStats.DrainRatePerSecond;
                previewMaxTime = baseStats.MaxEnergyTime;
                previewMoveSpeed = baseStats.BaseMoveSpeed;
            }
        }
        private void Start()
        {
            runtimeStats = new RuntimeStats(baseStats);

            Debug.Log($"Initial time: {runtimeStats.CurrentEnergyTime}");
            Debug.Log($"Drain/s: {runtimeStats.DrainRatePerSecond}");
            Debug.Log($"Max.Time: {runtimeStats.MaxEnergyTime}");
            Debug.Log($"Movement Speed: {runtimeStats.MoveSpeed}");
        }
    }
}
