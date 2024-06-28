using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    //public Rigidbody Rb { get; private set; }
    //public Animator Animator { get; private set; }
    [SerializeField] protected LayerMask whatIsPlayer;
    [Header("Stun Info")] 
    public float stunDuration;
    public Vector3 stunDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;
    
    [Header("Move info")] 
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    private float defaultMoveSpeed;

    [Header("Attack info")] 
    public float attackDistance;
    public float attackCooldown;
    public float lastTimeAttacked;
    public EnemyStateMachine StateMachine { get; private set; }
    public string lastAnimName { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.CurrentState.Update();
    }

    public virtual void AssignLastAnimName(string _animBoolName)
    {
        lastAnimName = _animBoolName;
    }

    public virtual void FreezeTime(bool isFrozen)
    {
        if (isFrozen)
        {
            moveSpeed = 0f;
            Anim.speed = 0f;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            Anim.speed = 1f;
        }
    }

    protected virtual IEnumerator FreezeTimeFor(float seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(seconds);
        FreezeTime(false);
    }

    public override void SlowEntityBy(float slowPercentage, float slowDuration)
    {
        moveSpeed = moveSpeed * (1 - slowPercentage);
        Anim.speed = Anim.speed * (1 - slowPercentage);
        
        Invoke(nameof(ReturnToDefaultSpeed), slowDuration);
    }

    protected override void ReturnToDefaultSpeed()
    {
        base.ReturnToDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
    }

    public virtual void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public virtual bool IsPlayerDetected() => Physics.Raycast(wallCheck.position, Vector3.right * FacingDirection, 50, whatIsPlayer);
    
    public virtual RaycastHit PlayerDetectedInfo()
    {
        Physics.Raycast(wallCheck.position, Vector3.right * FacingDirection, out RaycastHit hit, 50, whatIsPlayer);
        return hit;
    }

    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * FacingDirection, transform.position.y));
    }
}
