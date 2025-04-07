using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyModel : EnemyBase, IDamageable
{

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

 

    private void Start()
    {
        CurrentHealth = MaxHealth;
              
    }

    public void Damage(float damageAmount)
    {

        if (RastroOrbOnHit)
        {
            
            SpawnHealingOrb();

        }

        CurrentHealth -= damageAmount;
        OnHealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (RastroOrbOnDeath)
        {
            //Al morir se instancian 2 orbes
            SpawnHealingOrb();
            SpawnHealingOrb();
        }

        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    public void SpawnHealingOrb()
    {
        RastroOrb orb = orbPool.Get();

        //Se instancian detras del enemigo
        Vector3 spawnPos = transform.position - transform.forward * 1.5f + Vector3.up * 0.5f;
        orb.transform.position = spawnPos;

        orb.Init(() => orbPool.Release(orb));
    }
}

