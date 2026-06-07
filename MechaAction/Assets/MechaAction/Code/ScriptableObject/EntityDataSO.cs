using UnityEngine;

public enum StatusType
{
    HP,
    Shield,
    Speed,
    DashSpeed,
    Jump,
}

[CreateAssetMenu(fileName = "EntityData", menuName = "ScriptableObjects/Data/EntityData")]
public class EntityDataSO : ScriptableObject
{
    [SerializeField] private float m_maxHP;
    [SerializeField] private float m_shield;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_dashSpeed;
    [SerializeField] private float m_jump;

    public float MaxHP { get => m_maxHP; }
    public float Shield { get => m_shield; }
    public float Speed { get => m_speed; }
    public float DashSpeed { get => m_dashSpeed; }
    public float Jump { get => m_jump; }
}
