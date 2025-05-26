using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, ITriggerCheck
{

    private EnemyModel model;
    private EnemyView view;
    private Rigidbody rb;

    private NavMeshAgent _navMeshAgent;

    public Transform firePoint;


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
    public EnemyDeadState DeadState { get; set; }
    public EnemyEscapeState EscapeState { get; set; }
    public EnemyHurtState HurtState { get; set; }




    public enum InitialState
    {
        Patrol,
        Chase,
        Attack,
        Search,
        Idle,
        Stunned,
        Dead,
        Escape
    }

    public InitialState initialState = InitialState.Patrol;


    #endregion


    #region Behaviours
    //Behaviour
    [Header("FSM-Behaviour ScriptableObjects")]
    [SerializeField] private EnemyIdleSOBase EnemyIdleSOBase;
    [SerializeField] private EnemyAttackSOBase EnemyAttackSOBase;
    [SerializeField] private EnemyChaseSOBase EnemyChaseSOBase;
    [SerializeField] private EnemyDeadSOBase EnemyDeadSOBase;
    [SerializeField] private EnemyPatrolSOBase EnemyPatrolSOBase;
    [SerializeField] private EnemyStunnedSOBase EnemyStunnedSOBase;
    [SerializeField] private EnemyEscapeSOBase EnemyEscapeSOBase;
    [SerializeField] private EnemyHurtSOBase EnemyHurtSOBase;




    public EnemyIdleSOBase EnemyIdleBaseInstance { get; set; }
    public EnemyAttackSOBase EnemyAttackBaseInstance { get; set; }
    public EnemyChaseSOBase EnemyChaseBaseInstance { get; set; }
    public EnemyDeadSOBase EnemyDeadBaseInstance { get; set; }
    public EnemyPatrolSOBase EnemyPatrolBaseInstance { get; set; }
    public EnemyStunnedSOBase EnemyStunnedBaseInstance { get; set; }
    public EnemyEscapeSOBase EnemyEscapeBaseInstance { get; set; }
    public EnemyHurtSOBase EnemyHurtBaseInstance { get; set; }




    #endregion

    void Awake()
    {
        //Behaviour
        EnemyIdleBaseInstance = Instantiate(EnemyIdleSOBase);
        EnemyAttackBaseInstance = Instantiate(EnemyAttackSOBase);
        EnemyChaseBaseInstance = Instantiate(EnemyChaseSOBase);
        EnemyDeadBaseInstance = Instantiate(EnemyDeadSOBase);
        EnemyPatrolBaseInstance = Instantiate(EnemyPatrolSOBase);
        EnemyStunnedBaseInstance = Instantiate(EnemyStunnedSOBase);
        EnemyEscapeBaseInstance = Instantiate(EnemyEscapeSOBase);
        EnemyHurtBaseInstance = Instantiate(EnemyHurtSOBase);





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
        DeadState = new EnemyDeadState(this, fsm);
        EscapeState = new EnemyEscapeState(this, fsm);
        HurtState = new EnemyHurtState(this, fsm);



    }


    private void Start()
    {
      

        //Behaviour
        EnemyIdleBaseInstance.Initialize(gameObject, this);
        EnemyAttackBaseInstance.Initialize(gameObject, this);
        EnemyChaseBaseInstance.Initialize(gameObject, this);
        EnemyDeadBaseInstance.Initialize(gameObject, this);
        EnemyPatrolBaseInstance.Initialize(gameObject, this);
        EnemyStunnedBaseInstance.Initialize(gameObject, this);
        EnemyEscapeBaseInstance.Initialize(gameObject, this);
        EnemyHurtBaseInstance.Initialize(gameObject, this);



        InitializeState();



        _navMeshAgent = GetComponent<NavMeshAgent>();


        model.OnHealthChanged += HandleHealthChanged;
        model.OnDeath += HandleDeath;

       

       
    }

    private void Update()
    {
        fsm.CurrentEnemyState.FrameUpdate();

        Debug.Log(fsm.CurrentEnemyState);

        //Animacion de Movimiento
        view.PlayMovingAnimation(_navMeshAgent.speed);
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
            case InitialState.Dead:
                fsm.Initialize(DeadState);
                break;
            case InitialState.Escape:
                fsm.Initialize(EscapeState);
                break;


        }
    }

    //Realiza el ataque desde el eventtrigger de la animacion
    public void ExecuteAttack(IDamageable target)
    {
     
        target.TakeDamage(model.statsSO.AttackDamage);


    }

    public float GetDamage()
    {
        return model.statsSO.AttackDamage;
    }
    public void DoAttack(IDamageable target)
    {
        target.TakeDamage(GetDamage());
        Debug.Log("Daño hecho por el estado Melee");
    }

    private void HandleHealthChanged(float currentHealth)
    {
        float healthPercentage = currentHealth / model.statsSO.MaxHealth;

        //Cuando lo hieren pasa a Hurt
        fsm.ChangeState(HurtState);
    }

    private void HandleDeath(EnemyModel enemy)
    {
        fsm.ChangeState(DeadState);
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
