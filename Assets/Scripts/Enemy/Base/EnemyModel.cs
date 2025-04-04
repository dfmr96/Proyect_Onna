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
        CurrentHealth -= damageAmount;
        OnHealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }


}

