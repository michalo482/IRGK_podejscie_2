using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMonsterAttackState : EnemyState
{
    private EnemyBigMonster enemyBigMonster;
    public BigMonsterAttackState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, EnemyBigMonster enemyBigMonster) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemyBigMonster = enemyBigMonster;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        enemyBigMonster.ZeroVelocity();
        if (triggerCalled)
        {
            stateMachine.ChangeState(enemyBigMonster.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemyBigMonster.lastTimeAttacked = Time.time;
    }
}
