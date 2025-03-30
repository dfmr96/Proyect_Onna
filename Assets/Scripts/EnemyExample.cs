using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyExample : MonoBehaviour, IDamageable
{
    public Action<Vector3> OnDie;
    public float MaxHealth { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float CurrentHealth { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    private float _currentHealth = 1;


    public void Damage(float damageAmount) 
    {
        _currentHealth -= damageAmount;
        if (_currentHealth <= 0) Die();
        Debug.Log("Am beeing damaged");
    }

    public void Die() 
    {
        OnDie?.Invoke(transform.position);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage(UnityEngine.Random.Range(.25f, 1f));
        }
    }
}
