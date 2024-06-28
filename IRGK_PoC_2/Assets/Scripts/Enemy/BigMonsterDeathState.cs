using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMonsterDeathState : EnemyState
{
    private EnemyBigMonster enemyBigMonster;
    
    public BigMonsterDeathState(Enemy _enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, EnemyBigMonster enemyBigMonster) : base(_enemyBase, enemyStateMachine, animBoolName)
    {
        this.enemyBigMonster = enemyBigMonster;
    }

    public override void Enter()
    {
        base.Enter();
        enemyBigMonster.Anim.SetBool(enemyBigMonster.lastAnimName, true);
        enemyBigMonster.Anim.speed = 0;
        enemyBigMonster.CapsuleCollider.enabled = false;
        enemyBigMonster.stateTimer = 0.1f;
    }

    public override void Update()
    {
        base.Update();
        if (enemyBigMonster.stateTimer > 0)
        {
            rb.velocity = new Vector3(0, 5);
            rb.transform.Translate(0, 0, 1);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
