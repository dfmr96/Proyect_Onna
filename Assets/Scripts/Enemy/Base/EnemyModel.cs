using System;
using System.Collections;
using System.Collections.Generic;
using Enemy.Spawn;
using UnityEngine;


public class EnemyModel : MonoBehaviour, IDamageable
{
    [Header("Stats Config")]
    public EnemyStatsSO statsSO;

    public event Action<float> OnHealthChanged;
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }

    public event Action<EnemyModel> OnDeath;

    private EnemyView view;
    private EnemyController enemy;
    private OrbSpawner orbSpawner;



    private void Start()
    {
        MaxHealth = statsSO.MaxHealth;
        CurrentHealth = MaxHealth;

        view = GetComponent<EnemyView>();
        enemy = GetComponent<EnemyController>();
        orbSpawner = GameManager.Instance.orbSpawner;
    }

    public void TakeDamage(float damageAmount)
    {
        if (enemy.GetShield()) return;

        Debug.Log("Damagen received: " + damageAmount);
        if (statsSO.RastroOrbOnHit && orbSpawner != null)
        {
            for (int i = 0; i < statsSO.numberOfOrbsOnHit; i++)
            {
                orbSpawner.SpawnHealingOrb(transform.position, transform.forward);
            }
        }

        CurrentHealth -= damageAmount;
        OnHealthChanged?.Invoke(CurrentHealth);
        //view.PlayDamageAnimation();

        if (CurrentHealth <= 0) Die();
    }

    public void Die()
    {
        if (statsSO.RastroOrbOnDeath && orbSpawner != null)
        {
            for (int i = 0; i < statsSO.numberOfOrbsOnDeath; i++)
            {
                orbSpawner.SpawnHealingOrb(transform.position, transform.forward);
            }
        }
        RunData.CurrentCurrency.AddCoins(statsSO.CoinsToDrop);
        OnDeath?.Invoke(this);
    }

  
}

