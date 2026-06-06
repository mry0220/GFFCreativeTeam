using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class InputClass : IInputProvide
{
    //InputSystem----------------
    private CommandInput m_action;

    private Vector2 m_move;
    private bool m_IsJumped;

    public void Enable()
    {
        m_action = new CommandInput();

        m_action.Player.Move.performed += InputMove;
        m_action.Player.Move.canceled += InputMove;

        m_action.Player.Jump.performed += InputJump;

        m_action.Enable();
    }

    public void Disable()
    {
        m_action.Disable();
    }

    private void InputMove(InputAction.CallbackContext context)
    {
        m_move = context.ReadValue<Vector2>();
    }

    private void InputJump(InputAction.CallbackContext context)
    {
        m_IsJumped = true;
    }

    public Vector2 Move
    {
        get
        {
            return m_move;
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
}
