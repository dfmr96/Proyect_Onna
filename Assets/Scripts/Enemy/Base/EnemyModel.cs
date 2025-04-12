using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyModel : EnemyBase, IDamageable
{

    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    private EnemyView view;


    private void Start()
    {
        CurrentHealth = MaxHealth;
        view = GetComponent<EnemyView>();


    }

    public void Damage(float damageAmount)
    {

        if (RastroOrbOnHit)
        {
            
            SpawnHealingOrb();

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

