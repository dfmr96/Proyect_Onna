using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStunnedState : EnemyState
{
    private EnemyView _enemyView;
    private NavMeshAgent _navMeshAgent;
    private float _timer;
    private float _timeStun = 5f;


    public EnemyStunnedState(EnemyController enemy, EnemyStateMachine fsm) : base(enemy, fsm)
    {
        _navMeshAgent = enemy.GetComponent<NavMeshAgent>();

    }

    public override void EnterState()
    {
        base.EnterState();
        _enemyView = enemy.GetComponent<EnemyView>();


        _enemyView.PlayStunnedAnimation();

    }



    public override void ExitState()
    {
        base.ExitState();
        _timer = 0f;
        fsm.ChangeStateDirect(enemy.SearchState);

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        _timer += Time.deltaTime;

        if (_timer >= _timeStun)
        {

            fsm.ChangeState(enemy.ChaseState);

        }

    }


}
