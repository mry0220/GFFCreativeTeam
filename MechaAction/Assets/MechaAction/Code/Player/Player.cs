using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerAttackMode
{
    Sword,
    Gun
}

public class Player : Entity
{
  

    //component-----------------------
    private PlayerAttack m_attack;
    //--------------------------------

    //PlayerState---------------------
    private bool m_canDoubleJump;

    private bool m_IsDashed;
    //--------------------------------

    //PlayerStateTimer----------------
    private float m_TimeDashed;
    //--------------------------------

    // input action script
    private IInputProvide m_input;

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

    protected override void Update()
    {
        //player state change return when dash
        if(m_TimeDashed > 0)
        {
            m_TimeDashed -= Time.deltaTime;

            if(m_TimeDashed <= 0)
            {
                m_canActive = true;
                m_IsInvincible = false;
            }
        }

        base.Update();

        m_input.Update();

        m_moveDir = m_input.Move;
        m_IsJumped = m_input.IsJump;
        m_IsDashed = m_input.IsDashed;
        m_IsRunning = m_input.IsRun;
        m_frontDir = m_input.FrontDir;

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

        if(m_IsDashed)
        {

            OnDash();
        }
        

        if (m_IsJumped)
        {
            OnJump();

        }

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnDoubleJump()
    {
        m_fallTime = 0f;
        m_rb.linearVelocity = Vector2.zero;
        m_rb.AddForce(Vector3.up * Jump, ForceMode.Impulse);
    }

    private void OnDash()
    {
        m_IsInvincible = true;
        m_canActive = false;
        Debug.Log(Forward);

        m_rb.AddForce(Forward * 30f, ForceMode.Impulse);

        m_TimeDashed = 0.2f;
    }

    public void OnNormalAttack()
    {
        m_attack.OnAttack(PlayerAttackType.NormalAttack);
    }

    public void OnHadouken()
    {
        m_attack.OnAttack(PlayerAttackType.VoltSlash);
    }

    public void OnShouryuken()
    {
        m_attack.OnAttack(PlayerAttackType.GrandSlash);
    }
}
