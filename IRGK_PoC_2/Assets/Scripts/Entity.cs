using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision Info")] 
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("Knockback Info")] 
    [SerializeField] protected Vector3 knockbackDirection;
    [SerializeField] protected float knockbackDelay = 0.07f;
    protected bool isKnocked;
    
    protected const int Multiplayer = -1;
    public int FacingDirection { get; private set; } = 1;
    protected bool _facingRight = true;
    
    public Animator Anim { get; private set; }
    public Rigidbody Rb { get; private set; }
    public EntityFx fx { get; private set; }
    public SkinnedMeshRenderer Smr { get; private set; }
    
    public CharacterStats Stats { get; private set; }
    
    public CapsuleCollider CapsuleCollider { get; private set; }
    
    public float stateTimer;
    public float stateCooldown;

    public System.Action onFlipped;
    
    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        Smr = GetComponentInChildren<SkinnedMeshRenderer>();
        fx = GetComponentInChildren<EntityFx>();
        Anim = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody>();
        Stats = GetComponent<CharacterStats>();
        CapsuleCollider = GetComponent<CapsuleCollider>();
    }

    protected virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        stateCooldown -= Time.deltaTime;
    }

    public virtual void DamageEffects()
    {
        
        StartCoroutine("HitKnockback");
        //Debug.Log("jeb w ryj " + gameObject.name);
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        Rb.velocity = new Vector3(knockbackDirection.x * -FacingDirection, knockbackDirection.y);
        yield return new WaitForSeconds(knockbackDelay);
        isKnocked = false;
    }

    public virtual bool IsGroundDetected() =>
        Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, whatIsGround);

    public virtual bool IsWallDetected() =>
        Physics.Raycast(wallCheck.position, Vector3.right * FacingDirection, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        var groundCheckPosition = groundCheck.position;
        var wallCheckPosition = wallCheck.position;
        Gizmos.DrawLine(groundCheckPosition, new Vector3(groundCheckPosition.x, groundCheckPosition.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheckPosition, new Vector3(wallCheckPosition.x + wallCheckDistance, wallCheckPosition.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    public virtual void SlowEntityBy(float slowPercentage, float slowDuration)
    {
        
    }

    protected virtual void ReturnToDefaultSpeed() => Anim.speed = 1;
    
    public virtual void Flip()
    {
        FacingDirection *= Multiplayer;
        var transform1 = transform;
        var currentScale = transform1.localScale;
        currentScale.x *= Multiplayer;
        transform1.localScale = currentScale;
        
        _facingRight = !_facingRight;

        if (onFlipped != null)
            onFlipped();
    }

    public virtual void FlipController(float x)
    {
        switch (x)
        {
            case > 0 when !_facingRight:
            case < 0 when _facingRight:
                Flip();
                break;
        }
    }

    public virtual void ZeroVelocity()
    {
        if(isKnocked)
            return;
        Rb.velocity = new Vector3(0, 0);
    }
        

    public virtual void SetVelocity(float xVelocity, float yVelocity)
    {
        if(isKnocked)
            return;
        
        Rb.velocity = new Vector3(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public void MakeTransparent(bool transparent)
    {
        if (transparent)
        {
            Smr.material.color = Color.clear;
        }
        else
        {
            Smr.material.color = Color.white;
        }
    }

    public virtual void Die()
    {
        
    }
}
