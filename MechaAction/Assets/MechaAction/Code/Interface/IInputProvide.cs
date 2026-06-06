using UnityEngine;

public interface IInputProvide
{
    public Vector2 Move { get; }
    public bool IsJump { get; }

    public void Enable();

    public void Disable();
}
