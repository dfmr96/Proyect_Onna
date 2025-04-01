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
    private float _timeBetweenAttacks = 1.5f;
    private float _initialAttackDelay = 0.3f;

    private float _exitTimer;
    private float _timeTillExit;
    private float _distanceToCountExit = 3f;

    private bool _hasAttackedOnce = false;

    private EnemyModel _enemyModel;

    public EnemyAttackState(EnemyController enemy, EnemyStateMachine fsm) : base(enemy, fsm)
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _navMeshAgent = enemy.GetComponent<NavMeshAgent>();

    }

    public override void EnterState()
    {
        base.EnterState();

        _enemyModel = enemy.GetComponent<EnemyModel>();

        _enemyModel.OnHealthChanged += HandleHealthChanged;


        _navMeshAgent.SetDestination(_playerTransform.position);
        _timer = 0f; 
        _hasAttackedOnce = false;



    }

    public override void ExitState()
    {
        base.ExitState();
        _enemyModel.OnHealthChanged -= HandleHealthChanged;

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (_playerTransform == null) return; 

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


        if (!_hasAttackedOnce)
        {
            if (_timer >= _initialAttackDelay)
            {
                Attack();
                _hasAttackedOnce = true;
                _timer = 0f; 
            }
        }
        else if (_timer >= _timeBetweenAttacks)
        {
            Attack();
            _timer = 0f;
        }
    }

   private void Attack()
    {
        //aplicar interfaz de ataque
        IDamageable damageablePlayer = _playerTransform.GetComponent<IDamageable>();
        enemy.ExecuteAttack(damageablePlayer);
    }

    private void HandleHealthChanged(float currentHealth)
    {
        if (_timer >= _initialAttackDelay)
        {
            fsm.ChangeState(enemy.StunnedState);
        }
    }
}
