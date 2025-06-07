using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Upgrades/New Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public string description;
    public Sprite icon;
    public int cost;
}
