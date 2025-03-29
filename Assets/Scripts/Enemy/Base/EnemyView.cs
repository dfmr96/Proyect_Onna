using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAttackAnimation()
    {
        //animator.SetTrigger("Attack");
    }

    public void PlayDeathAnimation()
    {
        //animator.SetTrigger("Die");
    }

    public void UpdateHealthBar(float healthPercentage)
    {
        //health bar logic
    }
}

