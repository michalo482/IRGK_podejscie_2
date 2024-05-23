using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        player.stateTimer = player.counterAttackDuration;
        player.Anim.SetBool("SuccessfulCounter", false);
    }

    public override void Update()
    {
        base.Update();
        player.ZeroVelocity();
        Collider[] colliders = Physics.OverlapSphere(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    player.stateTimer = 10;
                    player.Anim.SetBool("SuccessfulCounter", true);
                }
            }
        }

        if (player.stateTimer < 0 || triggerCalled)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public PlayerCounterAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
}
