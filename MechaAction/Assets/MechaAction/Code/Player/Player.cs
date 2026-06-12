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
        base.Update();

        m_input.Update();

        m_moveDir = m_input.Move;
        m_IsJumped = m_input.IsJump;
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

    public void OnNormalAttack()
    {
        m_attack.OnAttack(PlayerAttackType.NormalAttack);
    }

    public void OnHadouken()
    {
        m_attack.OnAttack(PlayerAttackType.Slash);
    }

    public void OnShouryuken()
    {
        m_attack.OnAttack(PlayerAttackType.GroundAttack);
    }
}
