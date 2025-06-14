using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damageAmount);
    void Die();
    //float MaxHealth { get; set; }
    //float CurrentHealth { get; set; }
    float MaxHealth { get; }
    float CurrentHealth { get; }
}
