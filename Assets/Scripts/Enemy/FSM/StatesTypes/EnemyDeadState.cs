using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeadState : EnemyState
{
    private EnemyView _enemyView;
    private NavMeshAgent _navMeshAgent;
    private CapsuleCollider _collider;

    private float _timer;
    private float animationTime = 4f;


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
        _timer = 0f;

        //Spawnear orbes?
    }

    public override void ExitState()
    {
        base.ExitState();
        
        DeathManager.Instance.DestroyObject(enemy.gameObject);

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();


        _timer += Time.deltaTime;

        if (_timer > animationTime)
        {
            _timer = 0f;
            ExitState();
        }
    }


}
