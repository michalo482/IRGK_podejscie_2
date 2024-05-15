using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.stateTimer = 0.4f;
        player.SetVelocity(5 * -player.FacingDirection, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.IdleState);
        }
        
        if (player.stateTimer < 0)
            stateMachine.ChangeState(player.AirState);
    }

    public override void Exit()
    {
        base.Exit();
        
    }
}
