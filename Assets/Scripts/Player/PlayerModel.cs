using System;
using Player.Stats;
using UnityEngine;

public class PlayerModel : MonoBehaviour, IDamageable, IHealable
{
    public static Action OnPlayerDie;
    public static Action<float> OnUpdateTime;

    [Header("Stats Config")] [SerializeField]
    private CharacterBaseStats baseStats;

    [SerializeField] private StatReferences statRefs;

    private RuntimeStats stats;

    public float CurrentTime { get; private set; }

    private void Awake()
    {
        stats = RunData.CurrentStats ?? new RuntimeStats(baseStats, statRefs);
        RunData.SetStats(stats);

        CurrentTime = stats.CurrentEnergyTime;
    }

    private void Update()
    {
        ApplyPassiveDrain();
    }

    private void ApplyPassiveDrain()
    {
        float damagePerFrame = DrainRate * Time.deltaTime;
        TakeDamage(damagePerFrame);
    }

    public void TakeDamage(float timeTaken)
    {
        CurrentTime -= timeTaken;
        ClampEnergy();
        OnUpdateTime?.Invoke(CurrentTime / stats.Get(statRefs.maxVitalTime));

        if (CurrentTime <= 0)
            Die();
    }

    public void RecoverTime(float timeRecovered)
    {
        CurrentTime = Mathf.Min(CurrentTime + timeRecovered, stats.Get(statRefs.maxVitalTime));
        ClampEnergy();
        OnUpdateTime?.Invoke(CurrentTime / stats.Get(statRefs.maxVitalTime));
    }

    private void ClampEnergy()
    {
        stats.SetCurrentEnergyTime(CurrentTime, stats.Get(statRefs.maxVitalTime));
    }

    public float Speed => stats.Get(statRefs.movementSpeed);
    public float DrainRate => stats.Get(statRefs.passiveDrainRate);

    public float MaxHealth => stats.Get(statRefs.maxVitalTime);
    public float CurrentHealth => CurrentTime;

    public void Die() => OnPlayerDie?.Invoke();
}