using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBigMonster : Enemy
{
    
    public BigMonsterIdleState idleState { get; private set; }
    public BigMonsterMoveState moveState { get; private set; }
    public BigMonsterBattleState battleState { get; private set; }
    public BigMonsterAttackState attackState { get; private set; }
    
    
    protected override void Awake()
    {
        base.Awake();
        idleState = new BigMonsterIdleState(this, StateMachine, "Idle", this);
        moveState = new BigMonsterMoveState(this, StateMachine, "Move", this);
        battleState = new BigMonsterBattleState(this, StateMachine, "Move", this);
        attackState = new BigMonsterAttackState(this, StateMachine, "Attack", this);
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }
}
