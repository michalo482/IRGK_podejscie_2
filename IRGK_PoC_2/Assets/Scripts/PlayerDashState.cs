using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        player.stateTimer = player.dashDuration;
    }

    public override void Update()
    {
        base.Update();
        
        player.SetVelocity(player.dashSpeed * player.FacingDirection, rb.velocity.y);
        
        
        if (player.stateTimer < 0)
        {
            if (rb.velocity.y == 0)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.AirState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(player.xInput * player.moveSpeed, rb.velocity.y);
    }
}
