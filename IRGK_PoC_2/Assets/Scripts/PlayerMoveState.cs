using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (player.xInput == 0)
        {
            stateMachine.ChangeState(player.IdleState);
        }

        player.FlipController(player.xInput);
            
        player.SetVelocity(player.xInput * player.moveSpeed, rb.velocity.y);
        
        
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
