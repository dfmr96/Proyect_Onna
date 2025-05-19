using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack-Ranged Attack Basic", menuName = "Enemy Logic/Attack Logic/Ranged Attack With Projectiles")]

public class EnemyAttackRanged : EnemyAttackSOBase
{

    [Header("Ranged-Attack Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootForce = 15f;
    [SerializeField] private float attackCooldown = 2f;

    private float lastAttackTime;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();


        _navMeshAgent.SetDestination(playerTransform.position);
        //_hasAttackedOnce = false;

    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        ResetValues();
    }

    public override void DoFrameUpdateLogic()
    {

        base.DoFrameUpdateLogic();

        //if (!_hasAttackedOnce)
        //{
        //    if (_timer >= _initialAttackDelay)
        //    {
        //        Attack();
        //        _hasAttackedOnce = true;
        //        _timer = 0f;
        //    }
        //}
        //else if (_timer >= _timeBetweenAttacks)
        //{
        //    Attack();

        //    _timer = 0f;
        //}

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance > _enemyModel.statsSO.AttackRange)
        {
            enemy.fsm.ChangeState(enemy.ChaseState);
            return;
        }

        // Si ya pasó el cooldown
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            ShootProjectile();
            lastAttackTime = Time.time;
        }

        // Mirar al jugador
        Vector3 lookDir = (playerTransform.position - transform.position).normalized;
        lookDir.y = 0f;
        transform.rotation = Quaternion.LookRotation(lookDir);
    }




    public override void Initialize(GameObject gameObject, EnemyController enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();

        _enemyView.PlayAttackAnimation(false);
        TriggerAttackColorEffect();

    }

    private void ShootProjectile()
    {
        GameObject proj = GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        Vector3 dir = (playerTransform.position - firePoint.position).normalized;

        rb.AddForce(dir * shootForce, ForceMode.Impulse);
    }

    //private void Attack()
    //{
    //}
}
