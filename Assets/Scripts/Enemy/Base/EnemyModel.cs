using System;
using System.Collections;
using System.Collections.Generic;
using Enemy.Spawn;
using UnityEngine;


public class EnemyModel : EnemyBase, IDamageable
{

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    private EnemyView view;
    private OrbSpawner orbSpawner;


    private void Start()
    {
        CurrentHealth = MaxHealth;
        view = GetComponent<EnemyView>();
        orbSpawner = FindObjectOfType<OrbSpawner>();

    }

    public void TakeDamage(float damageAmount)
    {


        if (RastroOrbOnHit && orbSpawner != null)
        {
            orbSpawner.SpawnHealingOrb(transform.position, transform.forward);
        }

        CurrentHealth -= damageAmount;
        OnHealthChanged?.Invoke(CurrentHealth);
        view.PlayDamageAnimation();

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
      

        if (RastroOrbOnDeath && orbSpawner != null)
        {
            //Al morir se instancian 2 orbes
            orbSpawner.SpawnHealingOrb(transform.position, transform.forward);
            orbSpawner.SpawnHealingOrb(transform.position, transform.forward);
        }

        OnDeath?.Invoke();
    }

 
}

