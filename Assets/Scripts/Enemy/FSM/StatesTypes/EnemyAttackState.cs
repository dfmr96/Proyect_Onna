using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAttackState : EnemyState
{
    private Transform _playerTransform;
    private NavMeshAgent _navMeshAgent;
    float distanceToPlayer;

    private float initialSpeed;

    private float _timer;
    private float _timeBetweenAttacks = 1.5f;

    //Delay para que no ataque de una y darle un poco mas de efecto
    private float _initialAttackDelay = 0.3f;
    private float _animationAttackDelay = 0.4f;

    private float _exitTimer;
    private float _timeTillExit;
    private float _distanceToCountExit = 3f;

    private bool _hasAttackedOnce = false;

    private EnemyModel _enemyModel;
    private EnemyView _enemyView;

    public EnemyAttackState(EnemyController enemy, EnemyStateMachine fsm) : base(enemy, fsm)
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _navMeshAgent = enemy.GetComponent<NavMeshAgent>();

    }

    public override void EnterState()
    {
        base.EnterState();

        initialSpeed = _navMeshAgent.speed;

        _navMeshAgent.speed = 0.5f;

        _enemyModel = enemy.GetComponent<EnemyModel>();
        _enemyView = enemy.GetComponent<EnemyView>();   
        _enemyModel.OnHealthChanged += HandleHealthChanged;


        _navMeshAgent.SetDestination(_playerTransform.position);
        _timer = 0f;
        _hasAttackedOnce = false;


    }

    public override void ExitState()
    {
        base.ExitState();
        _enemyModel.OnHealthChanged -= HandleHealthChanged;
        _enemyView.PlayAttackAnimation(false);
        _hasAttackedOnce = false;

        _navMeshAgent.speed = initialSpeed;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        _timer += Time.deltaTime;

        //Si el Player muere durante el atque el enemigo se pone en idle
        if (_playerTransform == null)
        {

            fsm.ChangeState(enemy.IdleState);
            return;
        }

        distanceToPlayer = Vector3.Distance(_playerTransform.position, enemy.transform.position);

        if (distanceToPlayer > _distanceToCountExit)
        {
            _exitTimer += Time.deltaTime;

            if(_exitTimer > _timeTillExit)
            {
                _enemyView.PlayAttackAnimation(false);
                fsm.ChangeState(enemy.SearchState);

            }
        }

        else
        {
            _exitTimer = 0f;
        }

        //Siempre mira al Player al atacar
        enemy.transform.LookAt(new Vector3(_playerTransform.position.x, enemy.transform.position.y, _playerTransform.position.z));

        //Movimiento mas suave (probar)
        //Quaternion targetRotation = Quaternion.LookRotation(_playerTransform.position - enemy.transform.position);
        //enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 5f);

       


        if (!_hasAttackedOnce)
        {
            if (_timer >= _initialAttackDelay)
            {
                Debug.Log("entroooo");
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
        //ejecuta primero la animacion
        _enemyView.PlayAttackAnimation(true);

        //corrutina para aplicar el dano despues de la animacion
        enemy.StartCoroutine(DelayedDamage(_animationAttackDelay));  
    }

    private IEnumerator DelayedDamage(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_playerTransform == null) yield break;

        float distanceToPlayer = Vector3.Distance(_playerTransform.position, enemy.transform.position);

        //Si se alejo no aplicar daño
        if (distanceToPlayer > _distanceToCountExit) yield break; 


        IDamageable damageablePlayer = _playerTransform.GetComponent<IDamageable>();
        enemy.ExecuteAttack(damageablePlayer);
        _enemyView.PlayAttackAnimation(false);
    }

    private void HandleHealthChanged(float currentHealth)
    {
        if (_timer >= _initialAttackDelay)
        {
            fsm.ChangeState(enemy.StunnedState);
        }
    }
}
