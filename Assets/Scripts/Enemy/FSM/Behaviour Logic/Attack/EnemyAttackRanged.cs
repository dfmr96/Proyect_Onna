using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack-Ranged Attack Basic", menuName = "Enemy Logic/Attack Logic/Ranged Attack With Projectiles")]

public class EnemyAttackRanged : EnemyAttackSOBase
{
    private bool _hasAttackedOnce;


    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        _timer = 0f;
        _hasAttackedOnce = false;



    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        ResetValues();
    }

    public override void DoFrameUpdateLogic()
    {

        base.DoFrameUpdateLogic();

        if (enemy.isWhitinCombatRadius)
        {
            enemy.fsm.ChangeState(enemy.EscapeState);
        }



        if (!_hasAttackedOnce)
        {
            if (_timer >= _initialAttackDelay)
            {
                ShootProjectile();
                _hasAttackedOnce = true;
                _timer = 0f;
            }
        }
        else if (_timer >= _timeBetweenAttacks)
        {
            //TriggerAttackColorEffect();

            ShootProjectile();
            _timer = 0f;
        }

        
    }




    public override void Initialize(GameObject gameObject, EnemyController enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();

        _enemyView.PlayAttackAnimation(false);
        //TriggerAttackColorEffect();
        _hasAttackedOnce = false;
 
    }

    private void ShootProjectile()
    {
        _enemyView.PlayAttackAnimation(true);
        //TriggerAttackColorEffect();

       
    }

 
}
