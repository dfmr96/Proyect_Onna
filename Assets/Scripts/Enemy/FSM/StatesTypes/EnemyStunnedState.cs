using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStunnedState : EnemyState
{

    private NavMeshAgent _navMeshAgent;
    private float _timer;
    private float _timeStun = 1f;


    public EnemyStunnedState(EnemyController enemy, EnemyStateMachine fsm) : base(enemy, fsm)
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
        _timer = 0f;


    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        _timer += Time.deltaTime;

        if (_timer >= _timeStun)
        {
            fsm.ChangeState(enemy.AttackState);

        }

    }


}
