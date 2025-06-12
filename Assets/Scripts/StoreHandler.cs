using System.Collections.Generic;
using Core;
using Player;
using Player.Stats.Meta;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreHandler : MonoBehaviour
{
    [SerializeField] private Image upgradeImage;
    [SerializeField] private TextMeshProUGUI upgradeName;
    [SerializeField] private TextMeshProUGUI upgradeDescription;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    [SerializeField] private List<GameObject> upgradeButtons;
    [SerializeField] private TextMeshProUGUI onnaFragments;
    private UpgradeData selectedData;
    private HubManager hub;
    
    private PlayerModel player;
    private PlayerModelBootstrapper playerModelBootstrapper;

    private void Start() { CheckUpgradesAvailables(); }

    private void OnEnable()
    {
        EventBus.Subscribe<PlayerModelBootstrapperSignal>(GetModelBoostrapper);
    }
    
    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerModelBootstrapperSignal>(GetModelBoostrapper);
    }

    public void OnUpgradeClicked(BuyUpgradeButton button)
    {
        upgradeImage.sprite = button.Data.icon;
        upgradeDescription.text = button.Data.description;
        upgradeName.text = button.Data.name;
        upgradeCost.text = button.Data.cost.ToString();
        selectedData = button.Data;
    }

    public void SetHubManager(HubManager hubManager) { hub = hubManager; }

    public void CloseStore()
    {
        MetaStatSaveSystem.Save(playerModelBootstrapper.MetaStats, playerModelBootstrapper.Registry);
        hub.CloseStore();
    }
    public void UpdateCurrencyStatus() { onnaFragments.text = "Onna Fragments: " + hub.PlayerWallet.Coins; }
    public void CheckUpgradesAvailables()
    {
        UpdateCurrencyStatus();
        foreach (GameObject button in upgradeButtons)
        {
            button.GetComponent<Button>().interactable = false;
            if (button.GetComponent<BuyUpgradeButton>().Data != null)
            {
                if (!hub.PlayerWallet.CheckCost(button.GetComponent<BuyUpgradeButton>().Data.cost))
                    button.GetComponent<Button>().interactable = true;
            }
        }
    }
    public void TryBuyUpgrade()
    {
        if (selectedData != null)
        {
            if (hub.PlayerWallet.TrySpend(selectedData.cost))
            {
                player = PlayerHelper.GetPlayer().GetComponent<PlayerModel>();
                Debug.Log($"Compraste mejora: {selectedData.upgradeName}");
                selectedData.upgradeEffect?.Apply(player.StatContext.Meta);
                hub.UpdateCoins();
                CheckUpgradesAvailables();
                // Do Something
            }
        }
    }
    
    private void GetModelBoostrapper(PlayerModelBootstrapperSignal signal)
    {
        playerModelBootstrapper = signal.Bootstrapper;
    }
}
