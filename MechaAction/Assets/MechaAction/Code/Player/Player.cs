using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerAttackMode
{
    Sword,
    Gun
}

public class Player : Entity
{

    //--------------------state---------------
    public enum EnumActionState
    {
        Idle,
        Walk,
        Run,
        Evade,
        Attack,
        Hit,
        Guard,
        AutoGuard,
        Down,
        Dead
    }

    public enum EnumPoseState
    {
        Stand,
        Crouch,
        Air
    }

    private EnumActionState m_actionState;
    public EnumActionState actionState => m_actionState;

    private EnumPoseState m_poseState;
    public EnumPoseState PoseState => m_poseState;
    //---------------------------------------------------

    //component-----------------------
    private PlayerAttack m_attack;
    //--------------------------------

    //PlayerState---------------------
    private bool m_canDoubleJump;

    //private bool m_IsDashed;

    private bool m_IsGuard;
    //--------------------------------

    //PlayerStateTimer----------------
    private float m_TimeEvade;
    //--------------------------------

    // input action script
    private InputClass m_input;

    protected override void Awake()
    {
        base.Awake();

        m_attack = GetComponent<PlayerAttack>();

        m_input = new InputClass();

    }

    private void OnEnable()
    {
        m_input.Enable();
    }

    private void OnDisable()
    {
        m_input.Disable();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        m_IsGrounded = IsGrounded();

        if (m_frontDir != 0)
        {
            float Yrot = m_frontDir > 0 ? 90f : 270f;
            transform.rotation = Quaternion.Euler(0, Yrot, 0);
        }

        OnPlayerStatusTimer();

        OnStateUpdate();

        m_input.Update();

        m_moveDir = m_input.Move;
        //m_IsJumped = m_input.IsJump;
        //m_IsDashed = m_input.IsDashed;
        //m_IsRunning = m_input.IsRun;
        m_frontDir = m_input.FrontDir;
        m_IsGuard = m_input.IsGuard;

        if (!m_IsGrounded)
        {
            if (m_canDoubleJump && m_IsJumped)
            {
                m_canDoubleJump = false;

                OnDoubleJump();
            }
        }
        else
        {
            m_canDoubleJump = true;
        }

        OnGuard();
        

        if (m_IsJumped)
        {


            CallJump();
        }

    }

    public void CallJump()
    {
        if (m_actionState == EnumActionState.Dead) return;

        if (m_actionState == EnumActionState.Down) return;

        if (m_actionState == EnumActionState.Evade) return;

        OnJump();
    }

    private void OnStateUpdate()
    {
        if (m_actionState == EnumActionState.Dead) return;

        if (m_actionState == EnumActionState.Down) return;

        if (m_actionState == EnumActionState.Attack) return;

        if (m_actionState == EnumActionState.Evade) return;

        //run - walk - idle
        if(m_IsRunning)
        {
            m_anim.SetFloat("move", 1f);
            OnChangeActionState(EnumActionState.Run);
        }
        else if(m_moveDir != Vector2.zero)
        {
            m_anim.SetFloat("move", 0.5f);

            OnChangeActionState(EnumActionState.Walk);
        }
        else
        {
            m_anim.SetFloat("move", 0f);

            OnChangeActionState(EnumActionState.Idle);
        }
    }

    private void FixedUpdate()
    {
        //rule is we do not have to change m_rb.velocity.y because y is use Addforce
        //if  direct move to y is nothing addForce
        m_velocity = m_rb.linearVelocity;

        CallMove(m_moveDir);
        OnGravity();

        m_rb.linearVelocity = m_velocity;
    }

    private void CallMove(Vector2 dir)
    {
        if (m_actionState == EnumActionState.Dead) return;

        if (m_actionState == EnumActionState.Down) return;

        if (m_actionState == EnumActionState.Evade) return;

        OnMove(dir);
    }

    private void OnPlayerStatusTimer()
    {
        //player state change return when dash
        if (m_TimeEvade > 0)
        {
            m_TimeEvade -= Time.deltaTime;

            if (m_TimeEvade <= 0)
            {
                //m_canActive = true;
                OnChangeActionState(EnumActionState.Idle);
                m_IsInvincible = false;
            }
        }
    }

    private void OnDoubleJump()
    {
        m_fallTime = 0f;
        m_rb.linearVelocity = Vector2.zero;
        m_rb.AddForce(Vector3.up * Jump, ForceMode.Impulse);
    }

    public void OnRun()
    {
        m_IsRunning = true;
    }

    public void OnResetRun()
    {
        m_IsRunning = false;
    }

    public void OnEvade()
    {
        if (m_actionState != EnumActionState.Evade) return;

        m_IsInvincible = true;
        OnChangeActionState(EnumActionState.Evade);
        //Debug.Log(Forward);

        m_rb.AddForce(Forward * 30f, ForceMode.Impulse);

        m_TimeEvade = 0.2f;
    }

    private void OnGuard()
    {
        if (m_input.IsGuard)
        {
            if (m_actionState != EnumActionState.Guard)
            {
                OnChangeActionState(EnumActionState.Guard);
            }
        }
        else
        {
            if (m_actionState == EnumActionState.Guard)
            {
                OnChangeActionState(EnumActionState.Idle);
            }
        }
    }

    public override bool CanTakeDamage()
    {
        if (base.CanTakeDamage()) return true;

        if (m_actionState == EnumActionState.Dead) return true;

        return false;
    }

    protected override void OnDead()
    {
        OnChangeActionState(EnumActionState.Dead);
    }

    private void OnChangeActionState(EnumActionState state)
    {
        if (state != EnumActionState.Run && state != EnumActionState.Attack)
        {
            OnResetRun();
        }

        m_actionState = state;
    }

    public void OnNormalAttack()
    {
        switch(m_input.AttackMode)
        {
            case PlayerAttackMode.Sword:
                m_attack.OnAttack(PlayerAttackType.NormalAttack, m_team, Forward);
                break;
            case PlayerAttackMode.Gun:
                m_attack.OnAttack(PlayerAttackType.NormalGun, m_team, Forward);
                break;
        }
    }

    public void OnHadouken()
    {
        m_attack.OnAttack(PlayerAttackType.VoltSlash, m_team, Forward);
    }

    public void OnShouryuken()
    {
        m_attack.OnAttack(PlayerAttackType.GrandSlash, m_team, Forward);
    }

    
}
