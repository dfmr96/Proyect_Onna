using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Chase-Chase Basic", menuName = "Enemy Logic/Chase Logic/Chase Basic")]

public class EnemyChaseBasic : EnemyChaseSOBase
{
    [SerializeField] private int _speedAgentMultiply;
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        _navMeshAgent.speed = _enemyModel.statsSO.moveSpeed * _speedAgentMultiply;
        _navMeshAgent.angularSpeed = _enemyModel.statsSO.rotationSpeed * _speedAgentMultiply; ;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();


    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        _navMeshAgent.SetDestination(playerTransform.position);

        if (!enemy.isAggroed)
        {
            enemy.fsm.ChangeState(enemy.SearchState);
        }

        if (enemy.isWhitinCombatRadius)
        {
            enemy.fsm.ChangeState(enemy.AttackState);
        }
    }

    public override void Initialize(GameObject gameObject, EnemyController enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}

