using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : EnemyState
{

    private NavMeshAgent _navMeshAgent;

    public EnemyIdleState(EnemyController enemy, EnemyStateMachine fsm) : base(enemy, fsm)
    {
        _navMeshAgent = enemy.GetComponent<NavMeshAgent>();

    }

    public override void EnterState()
    {
        base.EnterState();

     
        _navMeshAgent.isStopped = true;

    }

    public override void ExitState()
    {
        base.ExitState();

        _navMeshAgent.isStopped = false;

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

   
    }

   
}

