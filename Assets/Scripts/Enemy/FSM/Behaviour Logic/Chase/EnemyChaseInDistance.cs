using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase-Chase In Distance To Player", menuName = "Enemy Logic/Chase Logic/Chase In Distance To Player")]

public class EnemyChaseInDistance : EnemyChaseSOBase
{
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();



    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();

        ResetValues();


    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > minDistanceToPlayer)
        {
            // Solo perseguir si estamos muy lejos
            _navMeshAgent.SetDestination(playerTransform.position);
            _navMeshAgent.isStopped = false;
        }
        else
        {
            // Detenerse para no acercarse demasiado
            _navMeshAgent.ResetPath();
            _navMeshAgent.isStopped = true;

            // Opcional: mirar al jugador si querés que lo enfrente
            Vector3 lookDir = (playerTransform.position - transform.position).normalized;
            lookDir.y = 0f; // Evita rotar en vertical
            if (lookDir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 5f);
            }
        }

        // Lógica de transición
        //if (!enemy.isAggroed)
        //{
        //    enemy.fsm.ChangeState(enemy.SearchState);
        //}

        //if (enemy.isWhitinCombatRadius)
        //{
        //    enemy.fsm.ChangeState(enemy.AttackState);
        //}

        if (distanceToPlayer <= _enemyModel.statsSO.AttackRange)
        {
            enemy.fsm.ChangeState(enemy.AttackState);
        }

    }

    public override void Initialize(GameObject gameObject, EnemyController enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    
    }

}
