using UnityEngine;
using UnityEngine.InputSystem;

public class InputClass : IInputProvide
{
    //InputSystem----------------
    private CommandInput m_action;

    private Vector2 m_move;
    private bool m_IsDashed;
    private float m_TimeDashing;

    private bool m_IsJumped;
    private PlayerAttackMode m_attackMode = PlayerAttackMode.Sword;

    public void Enable()
    {
        m_action = new CommandInput();

        m_action.Player.Move.performed += InputMove;
        m_action.Player.Move.canceled += InputMove;
        m_action.Player.Move.performed += InputDash;
        m_action.Player.Move.canceled += InputDashCancel;

        m_action.Player.Jump.performed += InputJump;
        m_action.Player.ModeChange.performed += InputAttackModeChange;

        m_action.Enable();
    }

    public void Disable()
    {
        m_action.Disable();
    }

    public void Update()
    {
        if(m_TimeDashing > 0)
        {
            m_TimeDashing -= Time.deltaTime;

        }
    }

    private void InputMove(InputAction.CallbackContext context)
    {
        m_move = context.ReadValue<Vector2>();

       
    }

    private void InputDash(InputAction.CallbackContext context)
    {
        if (m_TimeDashing > 0)
        {
            m_IsDashed = true;
            m_TimeDashing = 0;
        }

        m_TimeDashing = 0.3f;
    }

    private void InputDashCancel(InputAction.CallbackContext context)
    {
        if(m_IsDashed)
        {
            m_IsDashed = false;
        }
    }

    private void InputJump(InputAction.CallbackContext context)
    {
        m_IsJumped = true;
    }

    private void InputAttackModeChange(InputAction.CallbackContext context)
    {
        switch(m_attackMode)
        {
            case PlayerAttackMode.Sword:
                m_attackMode = PlayerAttackMode.Gun;
                break;
            case PlayerAttackMode.Gun:
                m_attackMode = PlayerAttackMode.Sword;
                break;
        }
    }

    public Vector2 Move
    {
        get
        {
            return m_move;
        }
    }

    public bool IsDashed
    {
        get
        {
            return m_IsDashed;
        }
    }

    public bool IsJump
    {
        get
        {
            bool result = m_IsJumped;
            m_IsJumped = false;

            return result;
        }
    }

    public PlayerAttackMode AttackMode
    {
        get
        {
            return m_attackMode;
        }
    }
}
