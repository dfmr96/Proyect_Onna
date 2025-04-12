using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : EnemyBase
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAttackAnimation(bool isAttacking)
    {
        animator.SetBool("IsAttacking", isAttacking);
    
    }

    public void PlayIdleAnimation()
    {
        //animator.SetTrigger("Idle");
    }

    public void PlayMovingAnimation(float moveSpeed)
    {
        animator.SetFloat("MoveSpeed", moveSpeed);
    }

    public void PlayStunnedAnimation()
    {
        //animator.SetTrigger("Stunned");
    }

    public void PlayDamageAnimation()
    {
        animator.SetTrigger("IsDamaged");
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

