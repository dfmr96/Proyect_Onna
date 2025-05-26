using Player;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    [SerializeField] private LevelProgression levelProgression;
    [SerializeField] private PlayerSpawner spawner;
    private void Awake()
    {
        levelProgression.ResetProgress();
        if (RunData.CurrentCurrency != null)
            SaveSystem.SaveCoins(RunData.CurrentCurrency.Coins);
        RunData.Clear();
        spawner.SpawnPlayer();
        PlayerHelper.EnableInput();
    }
}
