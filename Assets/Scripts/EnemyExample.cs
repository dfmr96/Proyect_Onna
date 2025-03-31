using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyExample : MonoBehaviour, IDamageable
{
    public Action<GameObject> OnDie;
    public float MaxHealth { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float CurrentHealth { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    private float _currentHealth = 1;


    public void Damage(float damageAmount) 
    {
        _currentHealth -= damageAmount;
        if (_currentHealth <= 0) Die();
    }

    public void Die() 
    {
        OnDie?.Invoke(gameObject);
        Destroy(gameObject);
    }
}
