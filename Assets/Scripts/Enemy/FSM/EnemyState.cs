using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Enemy enemy;
    protected EnemyStateMachine fsm; 

    public EnemyState(Enemy enemy, EnemyStateMachine fsm)
    {
        this.enemy = enemy;
        this.fsm = fsm;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }

    public virtual void FrameUpdate() { }

    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent() { }


}
