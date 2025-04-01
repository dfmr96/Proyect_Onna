using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyModel : MonoBehaviour, IDamageable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public float AttackDamage = 5f;

    //Redundante resolver tomarlo del inspector o al reves
    public float AttackRange = 10f; 

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

