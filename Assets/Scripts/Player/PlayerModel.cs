using System;
using Player.Stats;
using UnityEngine;

public class PlayerModel: MonoBehaviour, IDamageable, IHealable
{
    public static Action OnPlayerDie;
    public static Action<float> OnUpdateTime;
    [SerializeField] private CharacterBaseStats data;
    [SerializeField] RuntimeStats stats;

    private float maxTime;
    public float CurrentTime {  get; private set; }
    public float TimeDrainRate { get; private set; }
    public float Speed { get; private set; }


    private void Awake()
    {
        RunData.Initialize(data);
        stats = RunData.CurrentStats;

        CurrentTime = stats.CurrentEnergyTime;
        maxTime = stats.MaxVitalTime;
        TimeDrainRate = stats.DrainRatePerSecond;
        Speed = stats.MovementSpeed;
    }

    public void TakeDamage(float timeTaken) 
    { 
        CurrentTime -= timeTaken;
        stats.SetCurrentEnergyTime(CurrentTime);
        OnUpdateTime?.Invoke(CurrentTime / maxTime);
        if (CurrentTime <= 0) Die();
    }

    public void RecoverTime(float timeRecovered) 
    {
        CurrentTime += timeRecovered;
        if (CurrentTime > maxTime) CurrentTime = maxTime;
        stats.SetCurrentEnergyTime(CurrentTime);
        OnUpdateTime?.Invoke(CurrentTime /maxTime);
    }
    
    public void SetTime(float quantity) { CurrentTime = quantity; }
    public void SetSpeed(float quantity) { Speed = quantity; }

    public float MaxHealth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public float CurrentHealth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public void Die() { OnPlayerDie?.Invoke(); }
}