using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{

    // player dir forward 
    private int m_frontDir; // 1 or -1

    //PlayerState---------------------
    private bool m_canDoubleJump;

    //--------------------------------

    // input action script
    private IInputProvide m_input;

    protected override void Awake()
    {
        base.Awake();

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

        m_moveDir = m_input.Move;
        m_IsJumped = m_input.IsJump;

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
        m_rb.AddForce(Vector3.up * m_jump, ForceMode.Impulse);
    }
}
