using UnityEngine;

public enum StatusType
{
    HP,
    Shield,
    Speed,
    DashSpeed,
    Jump,
    Attack,
}

[CreateAssetMenu(fileName = "EntityData", menuName = "ScriptableObjects/Data/EntityData")]
public class EntityDataSO : ScriptableObject
{
    [SerializeField] private int m_maxHP;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_dashSpeed;
    [SerializeField] private float m_jump;

    public int MaxHP { get => m_maxHP; }
    public float Speed { get => m_speed; }
    public float DashSpeed { get => m_dashSpeed; }
    public float Jump { get => m_jump; }
}
