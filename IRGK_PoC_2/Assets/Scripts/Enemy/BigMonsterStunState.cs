using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMonsterStunState : EnemyState
{
    private EnemyBigMonster enemyBigMonster;
    public BigMonsterStunState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, EnemyBigMonster enemyBigMonster) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemyBigMonster = enemyBigMonster;
    }

    public override void Enter()
    {
        base.Enter();
        enemyBigMonster.fx.InvokeRepeating("RedColorBlink", 0, 0.1f);
        enemyBigMonster.stateTimer = enemyBigMonster.stunDuration;
        rb.velocity = new Vector3(-enemyBigMonster.FacingDirection * enemyBigMonster.stunDirection.x, enemyBigMonster.stunDirection.y);
    }

    public override void Update()
    {
        base.Update();
        if (enemyBigMonster.stateTimer < 0)
        {
            stateMachine.ChangeState(enemyBigMonster.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemyBigMonster.fx.Invoke("CancelRedBlink", 0);
    }
}
