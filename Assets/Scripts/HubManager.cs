using Player;
using UnityEngine;
using TMPro;

public class HubManager : MonoBehaviour
{
    [SerializeField] private LevelProgression levelProgression;
    [SerializeField] private PlayerSpawner spawner;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private GameObject storePrefab;
    private GameObject storeInstance;
    public static HubManager Instance;
    public PlayerWallet PlayerWallet { private set; get; }
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
        SetupBasics();
    }

    private void SetupBasics()
    {
        levelProgression.ResetProgress();

        // Crear o cargar PlayerWallet
        if (PlayerWallet == null)
            PlayerWallet = new PlayerWallet();

        // Si venimos de una run con monedas las sumamos
        if (RunData.CurrentCurrency != null)
        {
            PlayerWallet.AddCoins(RunData.CurrentCurrency.Coins);
            RunData.Clear();
        }

        spawner.SpawnPlayer();
        PlayerHelper.EnableInput();
        UpdateCoins();
    }

    public void UpdateCoins() { currencyText.text = "Onna Fragments: " + PlayerWallet.Coins.ToString(); }
    public void OpenStore()
    {
        if (storeInstance != null) return;

        storeInstance = Instantiate(storePrefab);
        StoreHandler handler = storeInstance.GetComponent<StoreHandler>();
        handler.SetHubManager(this);
        PlayerHelper.DisableInput();
    }

    public void CloseStore()
    {
        if (storeInstance != null)
        {
            Destroy(storeInstance);
            storeInstance = null;
            PlayerHelper.EnableInput();
        }
    }
}
