using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : EnemyState
{

    private NavMeshAgent _navMeshAgent;
    private float initialSpeed;

    public EnemyIdleState(EnemyController enemy, EnemyStateMachine fsm) : base(enemy, fsm)
    {
        _navMeshAgent = enemy.GetComponent<NavMeshAgent>();

    }

    public override void EnterState()
    {
        base.EnterState();
        initialSpeed = _navMeshAgent.speed;
        _navMeshAgent.speed = 0;
        _navMeshAgent.isStopped = true;

    }

    public override void ExitState()
    {
        base.ExitState();
        _navMeshAgent.speed = initialSpeed;
        _navMeshAgent.isStopped = false;

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

   
    }

   
}

