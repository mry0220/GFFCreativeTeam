using UnityEngine;

public interface IInputProvide
{
    public Vector2 Move { get; }
    public bool IsRun { get; }
    public bool IsJump { get; }
    public PlayerAttackMode AttackMode { get; }
    public int FrontDir { get; }
    public void Enable();

    public void Disable();

    public void Update();
}
