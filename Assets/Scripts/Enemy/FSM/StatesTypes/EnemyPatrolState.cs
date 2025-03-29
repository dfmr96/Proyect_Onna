using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyState
{
    private Vector3 _targetPos;
    private NavMeshAgent _navMeshAgent;

    public EnemyPatrolState(EnemyController enemy, EnemyStateMachine fsm) : base(enemy, fsm)
    {
        _navMeshAgent = enemy.GetComponent<NavMeshAgent>();

    }

    public override void EnterState()
    {
        base.EnterState();

        _navMeshAgent.speed = enemy.moveSpeed;  
        _navMeshAgent.angularSpeed = enemy.rotationSpeed;

        _targetPos = GetRandomPointInSphere();

        _navMeshAgent.SetDestination(_targetPos);
        _navMeshAgent.isStopped = false;

    }

    public override void ExitState()
    {
        base.ExitState();

        //_navMeshAgent.isStopped = true;

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (enemy.isAggroed)
        {
            fsm.ChangeState(enemy.ChaseState);
            
        }


        //check if enemy approach target
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _targetPos = GetRandomPointInSphere();
            _navMeshAgent.SetDestination(_targetPos);
        }
    }

    private Vector3 GetRandomPointInSphere()
    {
        Vector3 randomPoint = UnityEngine.Random.insideUnitSphere * enemy.RandomMovementRange;
        randomPoint.y = enemy.transform.position.y;
        return enemy.transform.position + randomPoint;
    }
}
