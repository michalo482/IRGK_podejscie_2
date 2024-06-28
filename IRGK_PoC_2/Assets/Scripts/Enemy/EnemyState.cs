using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected Rigidbody rb;
    
    protected bool triggerCalled;
    private string _animBoolName;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = enemyStateMachine;
        this._animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        rb = enemyBase.Rb;
        enemyBase.Anim.SetBool(_animBoolName, true);
    }

    public virtual void Update()
    {
        
    }

    public virtual void Exit()
    {
        enemyBase.Anim.SetBool(_animBoolName, false);
        enemyBase.AssignLastAnimName(_animBoolName);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
