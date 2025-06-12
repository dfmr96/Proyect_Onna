using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class BuyUpgradeButton : MonoBehaviour
{
    [SerializeField] private UpgradeData data;
    [SerializeField] private Image img;
    [SerializeField] private Sprite lockSprite;
    public UpgradeData Data => data;
    private void Start() 
    {
        if (data != null) img.sprite = data.icon;
        else img.sprite = lockSprite;
    }
}
