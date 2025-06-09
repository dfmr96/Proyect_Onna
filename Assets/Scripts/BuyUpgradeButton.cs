using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyUpgradeButton : MonoBehaviour
{
    [SerializeField] private UpgradeData data;
    [SerializeField] private Image img;
    public UpgradeData Data => data;
    private void Start() { img.sprite = data.icon; }
}
