using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        /*if (player.dashButton > 0 && player.stateCooldown < 0)
        {
            stateMachine.ChangeState(player.DashState);
            player.stateCooldown = player.dashCooldown;
        }*/
        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.IdleState);
        }

        if (player.xInput != 0)
        {
            player.SetVelocity(player.moveSpeed * 0.8f * player.xInput, rb.velocity.y);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
