using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, ITriggerCheck
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 400f;
    public float RandomMovementRange = 30f;


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
    }


    private void Start()
    {
        model.OnHealthChanged += HandleHealthChanged;
        model.OnDeath += HandleDeath;

        fsm.Initialize(PatrolState);
    }

    private void Update()
    {
        fsm.CurrentEnemyState.FrameUpdate();

        Debug.Log(fsm.CurrentEnemyState);
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


    //#region Movement Functions
    //public virtual void MoveEnemy(Vector3 direction)
    //{
    //    Vector3 movement = direction.normalized * moveSpeed;
    //    rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    //}

    //public virtual void LookEnemy(Vector3 direction)
    //{
    //    if (direction != Vector3.zero)
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(direction);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    //    }
    //}

    //#endregion

}
