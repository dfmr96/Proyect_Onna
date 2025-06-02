using Player;
using UnityEngine;
using TMPro;

public class HubManager : MonoBehaviour
{
    [SerializeField] private LevelProgression levelProgression;
    [SerializeField] private PlayerSpawner spawner;
    [SerializeField] private TextMeshProUGUI currencyText;
    private void Awake()
    {
        levelProgression.ResetProgress();
        if (RunData.CurrentCurrency != null)
            SaveSystem.SaveCoins(RunData.CurrentCurrency.Coins);
        RunData.Clear();
        spawner.SpawnPlayer();
        PlayerHelper.EnableInput();
    }

    private void Start() { currencyText.text += SaveSystem.LoadData().totalCoins; }
}
