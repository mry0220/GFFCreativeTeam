using UnityEngine;

public class DebugEnemy : Entity
{
    //--------------------state---------------
    public enum EnumActionState
    {
        Idle,
        Walk,
        Attack,
        Hit,
        Down,
        Dead
    }

    public enum EnumAIState
    {
        Patrol,
        Chase,
        Air
    }

    private EnumActionState m_actionState;
    public EnumActionState actionState => m_actionState;

    private EnumAIState m_AIState;
    public EnumAIState AIState => m_AIState;
    //---------------------------------------------------

    //component-----------------------
    private DebugEnemyAttack m_attack;
    //--------------------------------

    //PlayerState---------------------
    //private bool m_canDoubleJump;

    //private bool m_IsDashed;
    //--------------------------------

    //PlayerStateTimer----------------
    //private float m_TimeDashed;
    //--------------------------------

    // input action script
    //private IInputProvide m_input;

    protected override void Awake()
    {
        base.Awake();

        m_attack = GetComponent<DebugEnemyAttack>();

        //m_input = new InputClass();

    }

    private void OnEnable()
    {
        //m_input.Enable();
    }

    private void OnDisable()
    {
        //m_input.Disable();
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

        //OnPlayerStatusTimer();

        OnStateUpdate();

        //m_input.Update();

        //m_moveDir = m_input.Move;
        //m_IsJumped = m_input.IsJump;
        //m_IsDashed = m_input.IsDashed;
        //m_IsRunning = m_input.IsRun;
        //m_frontDir = m_input.FrontDir;

        //if (!m_IsGrounded)
        //{
        //    if (m_canDoubleJump && m_IsJumped)
        //    {
        //        m_canDoubleJump = false;

        //        //OnDoubleJump();
        //    }
        //}
        //else
        //{
        //    m_canDoubleJump = true;
        //}

        //if (m_IsDashed)
        //{

        //    OnDash();
        //}


        //if (m_IsJumped)
        //{


        //    CallJump();
        //}

    }

    private void CallJump()
    {
        if (m_actionState == EnumActionState.Dead) return;

        if (m_actionState == EnumActionState.Down) return;

        OnJump();
    }

    private void OnStateUpdate()
    {
        if (m_actionState == EnumActionState.Dead) return;

        if (m_actionState == EnumActionState.Down) return;

        if (m_actionState == EnumActionState.Attack) return;

        //walk - idle
        if (m_moveDir != Vector2.zero)
        {
            OnChangeActionState(EnumActionState.Walk);
        }
        else
        {
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

        OnMove(dir);
    }

    private void OnPlayerStatusTimer()
    {
        //player state change return when dash
        //if (m_TimeDashed > 0)
        //{
        //    m_TimeDashed -= Time.deltaTime;

        //    if (m_TimeDashed <= 0)
        //    {
        //        //m_canActive = true;
        //        OnChangeActionState(EnumActionState.Idle);
        //        m_IsInvincible = false;
        //    }
        //}
    }

    //private void OnDoubleJump()
    //{
    //    m_fallTime = 0f;
    //    m_rb.linearVelocity = Vector2.zero;
    //    m_rb.AddForce(Vector3.up * Jump, ForceMode.Impulse);
    //}

    //private void OnDash()
    //{
    //    m_IsInvincible = true;
    //    OnChangeActionState(EnumActionState.Dash);
    //    //Debug.Log(Forward);

    //    m_rb.AddForce(Forward * 30f, ForceMode.Impulse);

    //    m_TimeDashed = 0.2f;
    //}

    public override bool CanTakeDamage()
    {
        if (base.CanTakeDamage()) return true;

        if (m_actionState == EnumActionState.Dead) return true;

        return false;
    }

    protected override void OnDead()
    {
        OnChangeActionState(EnumActionState.Dead);

        Destroy(gameObject);
    }

    private void OnChangeActionState(EnumActionState state)
    {
        m_actionState = state;
    }

    //public void OnNormalAttack()
    //{
    //    m_attack.OnAttack(PlayerAttackType.NormalAttack);
    //}

    //public void OnHadouken()
    //{
    //    m_attack.OnAttack(PlayerAttackType.VoltSlash);
    //}

    //public void OnShouryuken()
    //{
    //    m_attack.OnAttack(PlayerAttackType.GrandSlash);
    //}

    
}
