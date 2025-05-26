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
    private OrbSpawner orbSpawner;



    private void Start()
    {
        MaxHealth = statsSO.MaxHealth;
        CurrentHealth = MaxHealth;

        view = GetComponent<EnemyView>();
        orbSpawner = FindObjectOfType<OrbSpawner>();
    }

    public void TakeDamage(float damageAmount)
    {
        Debug.Log("Damagen received: " + damageAmount);
        if (statsSO.RastroOrbOnHit && orbSpawner != null)
        {
            orbSpawner.SpawnHealingOrb(transform.position, transform.forward);
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
            //Al morir se instancian 2 orbes
            orbSpawner.SpawnHealingOrb(transform.position, transform.forward);
            orbSpawner.SpawnHealingOrb(transform.position, transform.forward);
        }
        RunData.CurrentCurrency.AddCoins(statsSO.CoinsToDrop);
        OnDeath?.Invoke(this);
    }
}

