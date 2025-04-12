using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : IAttack
{
    private float damage;

    public MeleeAttack(float damage)
    {
        this.damage = damage;
    }

    public void ExecuteAttack(IDamageable target)
    {
        target.Damage(damage);
        //Debug.Log("Melee attack executed!");
    }
}

