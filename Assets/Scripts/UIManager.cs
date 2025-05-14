using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private UIData data;
    [SerializeField] private Image timeCircle;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }
    private void OnEnable() { PlayerModel.OnUpdateTime += UpdateTimeUI; }
    private void OnDisable() { PlayerModel.OnUpdateTime -= UpdateTimeUI; }
    private void UpdateTimeUI(float timePercent) 
    {
        timeCircle.fillAmount = timePercent;
        if (timePercent < data.TimeToHurry)
        {
            float pulse = 1 + Mathf.Sin(Time.time * 10f) * 0.05f;
            timeCircle.transform.localScale = new Vector3(pulse, pulse, 1);
            timeCircle.color = data.HurryColor;
        }
        else 
        {
            timeCircle.transform.localScale = Vector3.one;
            timeCircle.color = data.NormalColor;
        }
    }

}
