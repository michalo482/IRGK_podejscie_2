using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected readonly PlayerStateMachine stateMachine;
    protected readonly Player player;
    protected Rigidbody rb;

    protected bool triggerCalled;
    
    private readonly string _animBoolName;
    private static readonly int YVelocity = Animator.StringToHash("yVelocity");

    protected PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this._animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        player.Anim.SetBool(_animBoolName, true);
        rb = player.Rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        player.Anim.SetFloat(YVelocity, rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.Anim.SetBool(_animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
