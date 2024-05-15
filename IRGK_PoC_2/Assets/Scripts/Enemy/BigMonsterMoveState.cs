using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMonsterMoveState : BigMonsterGroundedState
{
    public BigMonsterMoveState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, EnemyBigMonster enemyBigMonster) : base(_enemyBase, enemyStateMachine, animBoolName, enemyBigMonster)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        enemyBigMonster.SetVelocity(enemyBigMonster.moveSpeed * enemyBigMonster.FacingDirection, rb.velocity.y);
        if (enemyBigMonster.IsWallDetected() || !enemyBigMonster.IsGroundDetected())
        {
            enemyBigMonster.Flip();
            stateMachine.ChangeState(enemyBigMonster.idleState);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }

}
