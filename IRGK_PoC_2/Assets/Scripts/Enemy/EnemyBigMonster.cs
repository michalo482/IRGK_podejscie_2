using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBigMonster : Enemy
{
    
    public BigMonsterIdleState idleState { get; private set; }
    public BigMonsterMoveState moveState { get; private set; }
    public BigMonsterBattleState battleState { get; private set; }
    public BigMonsterAttackState attackState { get; private set; }
    public BigMonsterStunState stunState { get; private set; }
    
    public BigMonsterDeathState deathState { get; private set; }
    
    
    protected override void Awake()
    {
        base.Awake();
        idleState = new BigMonsterIdleState(this, StateMachine, "Idle", this);
        moveState = new BigMonsterMoveState(this, StateMachine, "Move", this);
        battleState = new BigMonsterBattleState(this, StateMachine, "Move", this);
        attackState = new BigMonsterAttackState(this, StateMachine, "Attack", this);
        stunState = new BigMonsterStunState(this, StateMachine, "Stun", this);
        deathState = new BigMonsterDeathState(this, StateMachine, "Stun", this);
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.U))
        {
            StateMachine.ChangeState(stunState);
        }
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            StateMachine.ChangeState(stunState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();
        StateMachine.ChangeState(deathState);
    }
}
