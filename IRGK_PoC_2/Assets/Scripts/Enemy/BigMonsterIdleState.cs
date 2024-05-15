using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMonsterIdleState : BigMonsterGroundedState
{
    public BigMonsterIdleState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, EnemyBigMonster enemyBigMonster) : base(_enemyBase, enemyStateMachine, animBoolName, enemyBigMonster)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        enemyBigMonster.stateTimer = enemyBigMonster.idleTime;
    }

    public override void Update()
    {
        base.Update();
        if (enemyBigMonster.stateTimer < 0)
        {
            stateMachine.ChangeState(enemyBigMonster.moveState);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }

}
