using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (player.jumpButton > 0)
        {
            stateMachine.ChangeState(player.WallJumpState);
            return;
        }
        if (player.yInput < 0)
        {
            rb.velocity = new Vector3(0, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y * 0.7f);
        }
        base.Update();
        if (player.xInput != 0 && player.xInput != player.FacingDirection)
        {
            stateMachine.ChangeState(player.IdleState);
        }

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
