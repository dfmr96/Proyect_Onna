using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : IAttack
{
    private float damage;
    private float attackRange;

    public RangedAttack(float damage, float attackRange)
    {
        this.damage = damage;
        this.attackRange = attackRange;
    }

    public void ExecuteAttack(IDamageable target)
    {
     
        //float distanceToTarget = Vector3.Distance(target.transform.position, Camera.main.transform.position); 

        //if (distanceToTarget <= attackRange)
        //{
        //    target.Damage(damage);
        //    Debug.Log("Ranged attack executed!");
        //}
        //else
        //{
        //    Debug.Log("Target out of range!");
        //}
    }
}

