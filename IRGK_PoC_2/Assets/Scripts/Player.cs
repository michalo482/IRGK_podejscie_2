using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Info")] 
    public float moveSpeed;

    public float jumpForce;
    
    #region Components
    public Animator Anim { get; private set; }
    public Rigidbody Rb { get; private set; }

    #endregion

    #region States

    private PlayerStateMachine _stateMachine;
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    
    #endregion

    private void Awake()
    {
        _stateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, _stateMachine, "Idle");
        MoveState = new PlayerMoveState(this, _stateMachine, "Move");
        JumpState = new PlayerJumpState(this, _stateMachine, "Jump");
        AirState  = new PlayerAirState(this, _stateMachine, "Jump");
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
        _stateMachine.CurrentState.Update();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        Rb.velocity = new Vector3(xVelocity, yVelocity);
    }
}
