using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected readonly PlayerStateMachine stateMachine;
    protected readonly Player player;
    protected Rigidbody rb;

    protected float xInput;

    private readonly string _animBoolName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
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

        xInput = player.movement.action.ReadValue<float>();
        player.Anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.Anim.SetBool(_animBoolName, false);
    }
}
