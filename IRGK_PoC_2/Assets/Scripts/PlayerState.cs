using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected readonly PlayerStateMachine stateMachine;
    protected readonly Player player;
    protected Rigidbody rb;
    
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
    }

    public virtual void Update()
    {
        player.Anim.SetFloat(YVelocity, rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.Anim.SetBool(_animBoolName, false);
    }
}
