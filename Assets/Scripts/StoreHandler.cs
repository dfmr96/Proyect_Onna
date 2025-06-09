using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreHandler : MonoBehaviour
{
    [SerializeField] private Image upgradeImage;
    [SerializeField] private TextMeshProUGUI upgradeName;
    [SerializeField] private TextMeshProUGUI upgradeDescription;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    private UpgradeData selectedData;
    private HubManager hub;

    public void OnUpgradeClicked(BuyUpgradeButton button)
    {
        upgradeImage.sprite = button.Data.icon;
        upgradeDescription.text = button.Data.description;
        upgradeName.text = button.Data.name;
        upgradeCost.text = button.Data.cost.ToString();
        selectedData = button.Data;
    }

    public void SetHubManager(HubManager hubManager) { hub = hubManager; }
    public void CloseStore() { hub.CloseStore(); }
    public void TryBuyUpgrade()
    {
        if (hub.PlayerWallet.TrySpend(selectedData.cost))
        {
            Debug.Log($"Compraste mejora: {selectedData.upgradeName}");
            hub.UpdateCoins();
            // Do Something
        }
    }
}
