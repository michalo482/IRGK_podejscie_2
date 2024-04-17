using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [Header("Movement Info")] 
    public float moveSpeed;
    public float jumpForce;
    
    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;
    public float dashDirection;
    
    [Header("Collision Info")] 
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    private const int Multiplayer = -1;
    public int FacingDirection { get; private set; } = 1;
    private bool _facingRight = true;
    
    public float xInput;
    public float yInput;
    public float dashButton;

    public float stateTimer;
    public float stateCooldown;
    
    #region Components
    public Animator Anim { get; private set; }
    public Rigidbody Rb { get; private set; }
    public InputActionReference movement;
    public InputActionReference jump;
    public InputActionReference dash;

    #endregion

    #region States

    private PlayerStateMachine _stateMachine;
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    
    #endregion

    private void Awake()
    {
        _stateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, _stateMachine, "Idle");
        MoveState = new PlayerMoveState(this, _stateMachine, "Move");
        JumpState = new PlayerJumpState(this, _stateMachine, "Jump");
        AirState  = new PlayerAirState(this, _stateMachine, "Jump");
        DashState = new PlayerDashState(this, _stateMachine, "Dash");
    }

    private void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody>();
        moveSpeed = 9f;
        
        _stateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        stateTimer -= Time.deltaTime;
        stateCooldown -= Time.deltaTime;
        
        xInput = movement.action.ReadValue<float>();
        yInput = jump.action.ReadValue<float>();
        dashButton = dash.action.ReadValue<float>();
        _stateMachine.CurrentState.Update();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        FlipController(xVelocity);
        Rb.velocity = new Vector3(xVelocity, yVelocity);
        
    }

    public bool IsGroundDetected() =>
        Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, whatIsGround);
    private void OnDrawGizmos()
    {
        var groundCheckPosition = groundCheck.position;
        var wallCheckPosition = wallCheck.position;
        Gizmos.DrawLine(groundCheckPosition, new Vector3(groundCheckPosition.x, groundCheckPosition.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheckPosition, new Vector3(wallCheckPosition.x + wallCheckDistance, wallCheckPosition.y));
    }

    private void Flip()
    {
        FacingDirection *= Multiplayer;
        var transform1 = transform;
        var currentScale = transform1.localScale;
        currentScale.x *= Multiplayer;
        transform1.localScale = currentScale;
        _facingRight = !_facingRight;
    }

    public void FlipController(float x)
    {
        switch (x)
        {
            case > 0 when !_facingRight:
            case < 0 when _facingRight:
                Flip();
                break;
        }
    }

    public void CheckForDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _stateMachine.ChangeState(DashState);
        }
    }
}
