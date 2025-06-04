using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack-Charging attack Tank type", menuName = "Enemy Logic/Attack Logic/Charging attack Tank type")]
public class EnemyAttackCharge : EnemyAttackSOBase
{
    private bool _hasAttackedOnce = false;

    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float chargeDuration = 1f;

    private bool isCharging = false;
    private float chargeTimer = 0f;
    private Vector3 chargeDirection;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        enemy.SetShield(false);

        _navMeshAgent.SetDestination(playerTransform.position);
        _hasAttackedOnce = false;
        isCharging = false;

        _navMeshAgent.isStopped = true;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        ResetValues();
    }

    public override void DoFrameUpdateLogic()
    {

        base.DoFrameUpdateLogic();

        _timer += Time.deltaTime;

        if (!_hasAttackedOnce && _timer >= _initialAttackDelay)
        {
            StartCharge();
        }

        if (isCharging)
        {
            chargeTimer += Time.deltaTime;
            enemy.transform.position += chargeDirection * (chargeSpeed * Time.deltaTime);

            if (chargeTimer >= chargeDuration)
            {
                EndCharge();
            }
        }


    }

    public override void Initialize(GameObject gameObject, EnemyController enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();

        _enemyView.PlayAttackAnimation(false);
        _hasAttackedOnce = false;
        enemy.SetShield(true);

    }


    private void StartCharge()
    {
        if (playerTransform == null) return;

        chargeDirection = (playerTransform.position - enemy.transform.position).normalized;
        chargeTimer = 0f;
        isCharging = true;
        _enemyView.PlayAttackAnimation(true);
        _hasAttackedOnce = true;
        _timer = 0f;
    }

    private void EndCharge()
    {
        isCharging = false;
        _navMeshAgent.isStopped = true;
        _navMeshAgent.ResetPath();
        _navMeshAgent.velocity = Vector3.zero;

        _enemyView.PlayAttackAnimation(false);
        enemy.SetAggroChecksEnabled(false);

        enemy.fsm.ChangeState(enemy.IdleState);
    }



}
