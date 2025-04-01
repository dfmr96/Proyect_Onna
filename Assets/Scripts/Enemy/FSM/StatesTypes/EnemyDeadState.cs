using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyState
{
    public EnemyDeadState(EnemyController enemy, EnemyStateMachine fsm) : base(enemy, fsm)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        //Animacion
        //Delay
        //Spawneo
    }

    public override void ExitState()
    {
        base.ExitState();
        //Destroy GameObject
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }


}
