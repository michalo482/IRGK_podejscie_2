using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        
    }

    

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (player.dashButton > 0 && player.stateCooldown < 0)
        {
            player.dashDirection = player.xInput;
            if (player.dashDirection == 0)
                player.dashDirection = player.FacingDirection;
            stateMachine.ChangeState(player.DashState);
            player.stateCooldown = player.dashCooldown;
        }
        
        if (player.yInput > 0 && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.JumpState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
