using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector3(rb.velocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();
     
        if (player.dashButton > 0 && player.stateCooldown < 0)
        {
            stateMachine.ChangeState(player.DashState);
            player.stateCooldown = player.dashCooldown;
        }
        
        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.AirState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
