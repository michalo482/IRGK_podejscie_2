using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    //public Rigidbody Rb { get; private set; }
    //public Animator Animator { get; private set; }
    [SerializeField] protected LayerMask whatIsPlayer;
    [Header("Move info")] 
    public float moveSpeed;
    public float idleTime;
    public float battleTime;

    [Header("Attack info")] 
    public float attackDistance;
    public float attackCooldown;
    public float lastTimeAttacked;
    public EnemyStateMachine StateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.CurrentState.Update();
        //Debug.Log(IsPlayerDetected() + " widze cie kurwa");
    }

    public virtual void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public virtual bool IsPlayerDetected() => Physics.Raycast(wallCheck.position, Vector3.right * FacingDirection, 50, whatIsPlayer);
    
    public virtual RaycastHit PlayerDetectedInfo()
    {
        Physics.Raycast(wallCheck.position, Vector3.right * FacingDirection, out RaycastHit hit, 50, whatIsPlayer);
        return hit;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * FacingDirection, transform.position.y));
    }
}
