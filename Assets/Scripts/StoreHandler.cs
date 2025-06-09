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
    [SerializeField] private List<GameObject> upgradeButtons;
    private UpgradeData selectedData;
    private HubManager hub;

    private void Start() { CheckUpgradesAvailables(); }

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

    public void CheckUpgradesAvailables()
    {
        foreach (GameObject button in upgradeButtons)
        {
            if (!hub.PlayerWallet.CheckCost(button.GetComponent<BuyUpgradeButton>().Data.cost))
                button.GetComponent<Button>().interactable = false;
            else button.GetComponent<Button>().interactable = true;
        }
    }
    public void TryBuyUpgrade()
    {
        if (hub.PlayerWallet.TrySpend(selectedData.cost))
        {
            Debug.Log($"Compraste mejora: {selectedData.upgradeName}");
            hub.UpdateCoins();
            CheckUpgradesAvailables();
            // Do Something
        }
    }
}
