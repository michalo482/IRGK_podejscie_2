using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMonsterGroundedState : EnemyState
{
    protected EnemyBigMonster enemyBigMonster;
    protected Transform player;
    public BigMonsterGroundedState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, EnemyBigMonster enemyBigMonster) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemyBigMonster = enemyBigMonster;
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void Update()
    {
        base.Update();
        if (enemyBigMonster.IsPlayerDetected() || Vector2.Distance(enemyBigMonster.transform.position, player.position) < 5)
        {
            stateMachine.ChangeState(enemyBigMonster.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
