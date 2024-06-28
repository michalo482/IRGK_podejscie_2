using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;

    public PlayerCatchSwordState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        
        sword = player.sword.transform;
        if (player.transform.position.x > sword.position.x && player.FacingDirection == 1)
        {
            player.Flip();
        }
        else if (player.transform.position.x < sword.position.x && player.FacingDirection == -1)
        {
            player.Flip();
        }

        rb.velocity = new Vector3(player.swordReturnImpact * -player.FacingDirection, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.1f);
    }
}
