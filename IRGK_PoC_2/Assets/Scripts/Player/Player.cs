using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Player : Entity
{
    [Header("Attack Info")] 
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;
    
    public bool isBusy { get; private set; }
    [Header("Movement Info")] 
    public float moveSpeed;
    public float jumpForce;
    
    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDirection;
    
    

    [SerializeField] private float _velocity;
    
    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }
    
    
    public float xInput;
    [FormerlySerializedAs("yInput")] public float jumpButton;
    public float yInput; 
    public float dashButton;
    public bool attackButton;

    
    
    #region Components
    
    public InputActionReference movement;
    public InputActionReference jump;
    public InputActionReference dash;
    public InputActionReference upAndDown;
    public InputActionReference attack;

    #endregion

    #region States

    private PlayerStateMachine _stateMachine;
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    
    public PlayerPrimaryAttackState PrimaryAttackState { get; private set; }
    public PlayerCounterAttackState CounterAttackState { get; private set; }
    
    public PlayerAimSwordState AimSwordState { get; private set; }
    public PlayerCatchSwordState CatchSwordState { get; private set; }
    
    #endregion

    protected override void Awake()
    {
        base.Awake();
        _stateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, _stateMachine, "Idle");
        MoveState = new PlayerMoveState(this, _stateMachine, "Move");
        JumpState = new PlayerJumpState(this, _stateMachine, "Jump");
        AirState  = new PlayerAirState(this, _stateMachine, "Jump");
        DashState = new PlayerDashState(this, _stateMachine, "Dash");
        WallSlideState = new PlayerWallSlideState(this, _stateMachine, "WallSlide");
        WallJumpState = new PlayerWallJumpState(this, _stateMachine, "Jump");

        PrimaryAttackState = new PlayerPrimaryAttackState(this, _stateMachine, "Attack");
        CounterAttackState = new PlayerCounterAttackState(this, _stateMachine, "CounterAttack");

        AimSwordState = new PlayerAimSwordState(this, _stateMachine, "AimSword");
        CatchSwordState = new PlayerCatchSwordState(this, _stateMachine, "CatchSword");
    }

    protected override void Start()
    {
        base.Start();
        
        skill = SkillManager.instance;
        moveSpeed = 9f;
        
        _stateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();
        
        xInput = movement.action.ReadValue<float>();
        jumpButton = jump.action.ReadValue<float>();
        dashButton = dash.action.ReadValue<float>();
        yInput = upAndDown.action.ReadValue<float>();
        attackButton = attack.action.IsPressed();
        _stateMachine.CurrentState.Update();
        CheckForDashInput();
        _velocity = Rb.velocity.y;
    }

    public IEnumerator BusyFor(float seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(seconds);

        isBusy = false;
    }

    public void AssignNewSword(GameObject newSword)
    {
        sword = newSword;
    }

    public void ClearSword()
    {
        Destroy(sword);
    }
    
    public void AnimationTrigger() => _stateMachine.CurrentState.AnimationFinishTrigger();

    

    public void CheckForDashInput()
    {
        if (IsWallDetected())
            return;
        
        if (dashButton > 0 && SkillManager.instance.dash.CanUseSkill())
        {
            dashDirection = xInput;
            if (dashDirection == 0)
                dashDirection = FacingDirection;
            _stateMachine.ChangeState(DashState);
            //stateCooldown = dashCooldown;
        }
    }
}