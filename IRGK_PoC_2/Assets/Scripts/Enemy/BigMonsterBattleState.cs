using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMonsterBattleState : EnemyState
{
    private Transform player;
    private EnemyBigMonster enemyBigMonster;
    private int moveDir;
    //[SerializeField] private Transform player;
    
    public BigMonsterBattleState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, EnemyBigMonster enemyBigMonster) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemyBigMonster = enemyBigMonster;
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("jestem zajety");
        //player = GameObject.Find("Player").transform;
        player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();
        if (enemyBigMonster.IsPlayerDetected())
        {
            enemyBigMonster.stateTimer = enemyBigMonster.battleTime;
            if (enemyBigMonster.PlayerDetectedInfo().distance < enemyBigMonster.attackDistance)
            {
                if(CanAttack())
                    stateMachine.ChangeState(enemyBigMonster.attackState);
            }
        }
        else
        {
            if (enemyBigMonster.stateTimer < 0 || Vector2.Distance(player.position, enemyBigMonster.transform.position) > 10)
            {
                stateMachine.ChangeState(enemyBigMonster.idleState);
            }
        }
        
        if (player.position.x > enemyBigMonster.transform.position.x)
        {
            moveDir = 1;
        }
        else if (player.position.x < enemyBigMonster.transform.position.x)
        {
            moveDir = -1;
        }
        else
        {
            moveDir = 0;
        }
        
        enemyBigMonster.SetVelocity(enemyBigMonster.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time > enemyBigMonster.lastTimeAttacked + enemyBigMonster.attackCooldown)
        {
            enemyBigMonster.lastTimeAttacked = Time.time;
            return true;
        }
        
        return false;
    }
}
