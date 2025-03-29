using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAttackState : EnemyState
{
    private Transform _playerTransform;
    private NavMeshAgent _navMeshAgent;
    float distanceToPlayer;



    private float _timer;
    private float _timeBetweenAttacks = 2f;

    private float _exitTimer;
    private float _timeTillExit;
    private float _distanceToCountExit = 3f;

    public EnemyAttackState(EnemyController enemy, EnemyStateMachine fsm) : base(enemy, fsm)
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _navMeshAgent = enemy.GetComponent<NavMeshAgent>();

    }

    public override void EnterState()
    {
        base.EnterState();

        _navMeshAgent.SetDestination(_playerTransform.position);
        Attack();
            
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (_timer > _timeBetweenAttacks)
        {
            _timer = 0f;

            Attack();

        }

        //if (_navMeshAgent.remainingDistance > _distanceToCountExit)

        distanceToPlayer = Vector3.Distance(_playerTransform.position, enemy.transform.position);

        if (distanceToPlayer > _distanceToCountExit)
        {
            _exitTimer += Time.deltaTime;

            if(_exitTimer > _timeTillExit)
            {
                fsm.ChangeState(enemy.SearchState);

            }
        }

        else
        {
            _exitTimer = 0f;
        }


        _timer += Time.deltaTime;
    }

   private void Attack()
    {
        Debug.Log("Ghost has Bite You");

    }
}
