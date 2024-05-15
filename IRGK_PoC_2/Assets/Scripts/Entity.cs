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
    
    
    
    protected const int Multiplayer = -1;
    public int FacingDirection { get; private set; } = 1;
    protected bool _facingRight = true;
    
    public Animator Anim { get; private set; }
    public Rigidbody Rb { get; private set; }
    public EntityFx fx { get; private set; }
    
    public float stateTimer;
    public float stateCooldown;
    
    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        fx = GetComponentInChildren<EntityFx>();
        Anim = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        stateCooldown -= Time.deltaTime;
    }

    public virtual void Damage()
    {
        fx.StartCoroutine("FlashFx");
        Debug.Log("jeb w ryj " + gameObject.name);
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
    
    public virtual void Flip()
    {
        FacingDirection *= Multiplayer;
        var transform1 = transform;
        var currentScale = transform1.localScale;
        currentScale.x *= Multiplayer;
        transform1.localScale = currentScale;
        
        _facingRight = !_facingRight;
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
    
    public virtual void ZeroVelocity() => Rb.velocity = new Vector3(0, 0);

    public virtual void SetVelocity(float xVelocity, float yVelocity)
    {
        Rb.velocity = new Vector3(xVelocity, yVelocity);
        FlipController(xVelocity);
    }
}
