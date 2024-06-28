using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{

    private float flyTime = 0.4f;
    private bool skillUsed;
    public PlayerBlackholeState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        skillUsed = false;
        player.stateTimer = flyTime;
        //rb.mass = 0;
    }

    public override void Update()
    {
        base.Update();
        if (player.stateTimer > 0)
        {
            rb.velocity = new Vector3(0, 10);
        }

        if (player.stateTimer < 0)
        {
            rb.velocity = new Vector3(0, -0.1f);
            if (!skillUsed)
            {
                if(player.skill.blackhole.CanUseSkill())
                    skillUsed = true;
            }
        }

        if (player.skill.blackhole.BlackholeFinished())
        {
            stateMachine.ChangeState(player.AirState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.MakeTransparent(false);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
