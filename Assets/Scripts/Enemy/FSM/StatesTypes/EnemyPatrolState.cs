using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyState
{
    private Vector3 _targetPos;
    private NavMeshAgent _navMeshAgent;
    private EnemyModel _enemyModel;


    public EnemyPatrolState(EnemyController enemy, EnemyStateMachine fsm) : base(enemy, fsm)
    {
        _navMeshAgent = enemy.GetComponent<NavMeshAgent>();

    }

    public override void EnterState()
    {
        base.EnterState();
        _enemyModel = enemy.GetComponent<EnemyModel>();
        _enemyModel.OnHealthChanged += HandleHealthChanged;

        //Se obliga a que el enemigo camine
        _navMeshAgent.speed = _enemyModel.statsSO.moveSpeed - 2;  
        _navMeshAgent.angularSpeed = _enemyModel.statsSO.rotationSpeed;

        _targetPos = GetRandomPointInSphere();

        _navMeshAgent.SetDestination(_targetPos);
        _navMeshAgent.isStopped = false;

    }

    public override void ExitState()
    {
        base.ExitState();
        _enemyModel.OnHealthChanged -= HandleHealthChanged;
        _navMeshAgent.speed += 2;
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
        Vector3 randomPoint = UnityEngine.Random.insideUnitSphere * _enemyModel.statsSO.RandomMovementRange;
        randomPoint.y = enemy.transform.position.y;
        return enemy.transform.position + randomPoint;
    }

    private void HandleHealthChanged(float currentHealth)
    {
        //Si es lastimado durante la patrulla, pasa a perseguir al player
       
            fsm.ChangeState(enemy.ChaseState);



    }
}
