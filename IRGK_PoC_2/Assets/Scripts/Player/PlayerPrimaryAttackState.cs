using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int _comboCounter { get; private set; }
    private float _lastTimeAttacked;
    private float _comboWindow = 2f;
    
    public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //rb.velocity = new Vector3(0, 0);
        if (_comboCounter > 2 || Time.time > _lastTimeAttacked + _comboWindow)
            _comboCounter = 0;

        player.Anim.SetInteger("ComboCounter", _comboCounter);
        player.Anim.speed = 2;
        float attackDir = player.FacingDirection;
        if (player.xInput != 0)
        {
            attackDir = player.xInput;
        }
        player.SetVelocity(player.attackMovement[_comboCounter].x * attackDir, player.attackMovement[_comboCounter].y);

        player.stateTimer = 0.1f;
    }

    public override void Update()
    {
        base.Update();
        if (player.stateTimer < 0)
        {
            player.ZeroVelocity();
        }
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.Anim.speed = 1;
        player.StartCoroutine($"BusyFor", 0.15f);
        _comboCounter++;
        _lastTimeAttacked = Time.time;
    }
}
