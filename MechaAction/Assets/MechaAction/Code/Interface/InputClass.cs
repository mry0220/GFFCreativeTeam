using UnityEngine;
using UnityEngine.InputSystem;

public class InputClass : IInputProvide
{
    //InputSystem----------------
    private CommandInput m_action;

    private Vector2 m_move;
    private bool m_IsRun;
    private float m_TimeRightDashing;
    private float m_TimeLeftDashing;

    private bool m_IsJumped;
    private bool m_IsDashed;
    private PlayerAttackMode m_attackMode = PlayerAttackMode.Sword;
    private int m_frontDir = 1;

    public void Enable()
    {
        m_action = new CommandInput();

        m_action.Player.Move.performed += InputMove;
        m_action.Player.Move.canceled += InputMove;
        m_action.Player.Move.canceled += InputDashCancel;

        m_action.Player.MoveRight.performed += InputMoveRight;
        m_action.Player.MoveLeft.performed += InputMoveLeft;

        m_action.Player.Jump.performed += InputJump;
        m_action.Player.Dash.performed += InputDash;
        m_action.Player.ModeChange.performed += InputAttackModeChange;

        m_action.Player.Switch.performed += InputSwitch;

        m_action.Enable();
    }

    public void Disable()
    {
        m_action.Disable();
    }

    public void Update()
    {
        if(m_TimeRightDashing > 0)
        {
            m_TimeRightDashing -= Time.deltaTime;

        }

        if(m_TimeLeftDashing > 0)
        {
            m_TimeLeftDashing -= Time.deltaTime;
        }
    }

    private void InputMove(InputAction.CallbackContext context)
    {
        m_move = context.ReadValue<Vector2>();

       
    }

    private void InputMoveRight(InputAction.CallbackContext context)
    {
        if (m_TimeRightDashing > 0)
        {
            m_IsRun = true;
            m_TimeRightDashing = 0;
        }

        m_TimeLeftDashing = 0f;
        m_TimeRightDashing = 0.3f;
    }

    private void InputMoveLeft(InputAction.CallbackContext context)
    {
        if (m_TimeLeftDashing > 0)
        {
            m_IsRun = true;
            m_TimeLeftDashing = 0;
        }

        m_TimeRightDashing = 0f;
        m_TimeLeftDashing = 0.3f;
    }

    private void InputDashCancel(InputAction.CallbackContext context)
    {
        if(m_IsRun)
        {
            m_IsRun = false;
        }
    }

    private void InputJump(InputAction.CallbackContext context)
    {
        m_IsJumped = true;
    }

    private void InputDash(InputAction.CallbackContext context)
    {
        m_IsDashed = true;
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

    private void InputSwitch(InputAction.CallbackContext context)
    {
        if(m_frontDir == 1)
        {
            m_frontDir = -1;
        }
        else
        {
            m_frontDir = 1;
        }
    }

    public Vector2 Move
    {
        get
        {
            return m_move;
        }
    }

    public bool IsRun
    {
        get
        {
            return m_IsRun;
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

    public bool IsDashed
    {
        get
        {
            bool result = m_IsDashed;
            m_IsDashed = false;

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

    public int FrontDir
    {
        get
        {
            return m_frontDir;
        }
    }
}
