using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeadState : EnemyState
{
    private EnemyView _enemyView;
    private NavMeshAgent _navMeshAgent;
    private CapsuleCollider _collider;


    public EnemyDeadState(EnemyController enemy, EnemyStateMachine fsm) : base(enemy, fsm)
    {
        _navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        _enemyView = enemy.GetComponent<EnemyView>();
        _collider = enemy.GetComponent<CapsuleCollider>();

    }

    public override void EnterState()
    {
        base.EnterState();
        _navMeshAgent.speed = 0;
        _navMeshAgent.isStopped = true;
        _collider.enabled = false;
        _enemyView.PlayDeathAnimation();

        
        //Spawnear orbes
    }

    public override void ExitState()
    {
        base.ExitState();
        //Destroy GameObject
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }


}
