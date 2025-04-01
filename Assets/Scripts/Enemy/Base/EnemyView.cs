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
        Debug.Log("Enemy has attacked");
    }

    public void PlayIdleAnimation()
    {
        //animator.SetTrigger("Idle");
    }

    public void PlayMovingAnimation()
    {
        //animator.SetTrigger("Moving");
    }

    public void PlayStunnedAnimation()
    {
        //animator.SetTrigger("Stunned");
    }

    public void PlayDamageAnimation()
    {
        //animator.SetTrigger("Damage");
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

