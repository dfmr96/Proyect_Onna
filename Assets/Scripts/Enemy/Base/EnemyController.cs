using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EnemyBase, ITriggerCheck
{

    private IAttack attackStrategy;

    private EnemyModel model;
    private EnemyView view;
    private Rigidbody rb;

    public bool isAggroed { get; set; }
    public bool isWhitinCombatRadius { get; set; }

    #region State Machine Variables

    public EnemyStateMachine fsm { get; set; }
    public EnemyPatrolState PatrolState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }
    public EnemySearchState SearchState { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyStunnedState StunnedState { get; set; }

    public enum InitialState
    {
        Patrol,
        Chase,
        Attack,
        Search,
        Idle,
        Stunned
    }

    public InitialState initialState = InitialState.Patrol;


    #endregion


    void Awake()
    {
        model = GetComponent<EnemyModel>();
        view = GetComponent<EnemyView>();
        rb = GetComponent<Rigidbody>();

        fsm = new EnemyStateMachine();

        PatrolState = new EnemyPatrolState(this, fsm);
        ChaseState = new EnemyChaseState(this, fsm);
        AttackState = new EnemyAttackState(this, fsm);
        SearchState = new EnemySearchState(this, fsm);
        StunnedState = new EnemyStunnedState(this, fsm);
        IdleState = new EnemyIdleState(this, fsm);

    }


    private void Start()
    {
        model.OnHealthChanged += HandleHealthChanged;
        model.OnDeath += HandleDeath;

        InitializeState();

        if (isRangedAttack)
        {
            attackStrategy = new RangedAttack(model.AttackDamage, model.AttackRange);  
        }
        else
        {
            attackStrategy = new MeleeAttack(model.AttackDamage); 
        }
    }

    private void Update()
    {
        fsm.CurrentEnemyState.FrameUpdate();

        Debug.Log(fsm.CurrentEnemyState);
    }

    private void InitializeState()
    {
        switch (initialState)
        {
            case InitialState.Patrol:
                fsm.Initialize(PatrolState);
                break;
            case InitialState.Chase:
                fsm.Initialize(ChaseState);
                break;
            case InitialState.Attack:
                fsm.Initialize(AttackState);
                break;
            case InitialState.Search:
                fsm.Initialize(SearchState);
                break;
            case InitialState.Idle:
                fsm.Initialize(IdleState);
                break;
            case InitialState.Stunned:
                fsm.Initialize(StunnedState);
                break;
        }
    }

    public void ExecuteAttack(IDamageable target)
    {
        attackStrategy.ExecuteAttack(target);  
        view.PlayAttackAnimation();  
    }

    private void HandleHealthChanged(float currentHealth)
    {
        float healthPercentage = currentHealth / model.MaxHealth;
        view.UpdateHealthBar(healthPercentage);
    }

    private void HandleDeath()
    {
        view.PlayDeathAnimation();
        Destroy(gameObject, 1f); 
    }
    public virtual void Attack()
    {
        view.PlayAttackAnimation();
    }



    public void SetAggroStatus(bool IsAggroed)
    {
        isAggroed = IsAggroed;
    }

    public void SetCombatRadiusBool(bool IsWhitinCombatRadius)
    {
        isWhitinCombatRadius = IsWhitinCombatRadius;
    }



}
