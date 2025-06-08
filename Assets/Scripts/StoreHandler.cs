using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreHandler : MonoBehaviour
{
    [SerializeField] private List<UpgradeData> playerUpgrades;
    [SerializeField] private List<UpgradeData> weaponUpgrades;
    [SerializeField] private List<Button> playerUpgradeButtons;
    [SerializeField] private List<Button> weaponUpgradeButtons;
    [SerializeField] private List<TextMeshProUGUI> playerDescriptions;
    [SerializeField] private List<TextMeshProUGUI> weaponDescriptions;
    private HubManager hub;
    private int currentWeaponUpgradeLevel = 0;

    private void Start() { SetupButtons(); }

    private void SetupButtons()
    {
        for (int i = 0; i < playerUpgrades.Count && i < playerUpgradeButtons.Count; i++)
        {
            int index = i;
            var data = playerUpgrades[i];

            playerUpgradeButtons[i].onClick.RemoveAllListeners();
            SetButtonVisuals(playerUpgradeButtons[i], data, index);
            playerUpgradeButtons[i].onClick.AddListener(() => TryBuyUpgrade(data));
        }
        UpdateWeaponButtons();
    }

    private void UpdateWeaponButtons()
    {
        for (int i = 0; i < weaponUpgrades.Count && i < weaponUpgradeButtons.Count; i++)
        {
            var button = weaponUpgradeButtons[i];
            var data = weaponUpgrades[i];

            SetButtonVisuals(button, data, i + playerUpgrades.Count);

            if (i == currentWeaponUpgradeLevel)
            {
                button.interactable = true;
                int index = i;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => TryBuyWeaponUpgrade(data));
            }
            else button.interactable = false;
        }
    }

    private void SetButtonVisuals(Button button, UpgradeData data, int index)
    {
        var image = button.GetComponent<Image>();
        if (image != null)
            image.sprite = data.icon;

        if (index < playerUpgrades.Count)
        {
            if (index < playerDescriptions.Count)
                playerDescriptions[index].text = data.description;
        }
        else
        {
            int weaponIndex = index - playerUpgrades.Count;
            if (weaponIndex < weaponDescriptions.Count)
                weaponDescriptions[weaponIndex].text = data.description;
        }
    }

    public void SetHubManager(HubManager hubManager) { hub = hubManager; }

    public void CloseStore() { hub.CloseStore(); }

    private void TryBuyUpgrade(UpgradeData data)
    {
        if (hub.PlayerWallet.TrySpend(data.cost))
        {
            Debug.Log($"Compraste mejora: {data.upgradeName}");
            hub.UpdateCoins();
            // Do Something
        }
    }

    private void TryBuyWeaponUpgrade(UpgradeData data)
    {
        if (hub.PlayerWallet.TrySpend(data.cost))
        {
            Debug.Log($"Compraste mejora de arma: {data.upgradeName}");
            currentWeaponUpgradeLevel++;
            UpdateWeaponButtons();
            hub.UpdateCoins();
            // Do Something
        }
    }

}
