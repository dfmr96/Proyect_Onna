using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyState
{
    private Transform _playerTransform;
    private NavMeshAgent _navMeshAgent;

    public EnemyChaseState(EnemyController enemy, EnemyStateMachine fsm) : base(enemy, fsm)
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _navMeshAgent = enemy.GetComponent<NavMeshAgent>();
    }

    public override void EnterState()
    {
        base.EnterState();
        _navMeshAgent.speed = enemy.moveSpeed;
        _navMeshAgent.angularSpeed = enemy.rotationSpeed;
    }

    public override void ExitState()
    {
        base.ExitState();

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        _navMeshAgent.SetDestination(_playerTransform.position);

        if (!enemy.isAggroed)
        {
            fsm.ChangeState(enemy.SearchState);
        }

        if (enemy.isWhitinCombatRadius)
        {
            fsm.ChangeState(enemy.AttackState);
        }

    }
}
