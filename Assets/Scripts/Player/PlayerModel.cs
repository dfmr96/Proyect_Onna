using System;
using Player.Stats;
using UnityEngine;

public class PlayerModel: MonoBehaviour, IDamagable, IHealable
{
    public static Action OnPlayerDie;
    public static Action<float> OnUpdateTime;
    [SerializeField] private CharacterBaseStats data;

    private float maxTime;
    private float currentTime;
    public float TimeDrainRate { get; private set; }
    public float Speed { get; private set; }

    private void Start()
    {
        maxTime = data.StartEnergyTime;
        currentTime = maxTime;
        TimeDrainRate = data.DrainRatePerSecond;
        Speed = data.BaseMoveSpeed;
    }

    public void TakeDamage(float timeTaken) 
    { 
        currentTime -= timeTaken;
        OnUpdateTime?.Invoke(currentTime / maxTime);
        if (currentTime <= 0) OnPlayerDie?.Invoke();
    }

    public void RecoverTime(float timeRecovered) 
    {
        currentTime += timeRecovered;
        if (currentTime > maxTime) currentTime = maxTime;
        OnUpdateTime?.Invoke(currentTime/maxTime);
    }
}