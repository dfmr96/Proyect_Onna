using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    private Animator animator;
    private Transform _playerTransform;
    private EnemyController _enemyController;

    private float _distanceToCountExit = 3f;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        //_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _playerTransform = PlayerHelper.GetPlayer().transform;

        _enemyController = GetComponent<EnemyController>();


    }

    //ActionEvent de Ataque
    public void AnimationAttackFunc()
    {

        if (_playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(_playerTransform.position, transform.position);

            //doble comprobacion por si se aleja
            if (distanceToPlayer <= _distanceToCountExit)
            {
                IDamageable damageablePlayer = _playerTransform.GetComponent<IDamageable>();
                _enemyController.ExecuteAttack(damageablePlayer);

            }
        }
    }



    //Animaciones
    public void PlayAttackAnimation(bool isAttacking)
    {
        animator.SetBool("IsAttacking", isAttacking);
    }

    public bool GetBoolAttackAnimation()
    {
        return animator.GetBool("IsAttacking");
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
        animator.SetTrigger("IsStunned");
    }

    public void PlayDamageAnimation()
    {
        animator.SetTrigger("IsDamaged");
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger("IsDead");
     


    }

    public void UpdateHealthBar(float healthPercentage)
    {
        //health bar logic
    }
}

