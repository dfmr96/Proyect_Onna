using System;
using Player.Stats;
using UnityEngine;

public class PlayerModel: MonoBehaviour, IDamageable, IHealable
{
    public static Action OnPlayerDie;
    public static Action<float> OnUpdateTime;
    [SerializeField] private CharacterBaseStats data;

    private float maxTime;
    [SerializeField] float currentTime;
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



    public float MaxHealth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public float CurrentHealth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public void Die()
    {
        throw new NotImplementedException();
    }
}