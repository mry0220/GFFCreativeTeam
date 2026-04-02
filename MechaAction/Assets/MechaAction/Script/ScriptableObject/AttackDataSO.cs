using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "SctiptableObjects/AttackData")]
public class AttackDataSO : ScriptableObject
{
    [SerializeField] private string m_info;

    [SerializeField] private int m_damage;
    [SerializeField] private float m_criticalRate;
    [SerializeField, Range(0f, 100f)] private float m_criticalChance;
    [SerializeField] private int m_knockback;
    [SerializeField] private DamageType m_type;
    [SerializeField] private float m_duration;

    public int Damage { get => m_damage; }
    public float CriticalRate { get => m_criticalRate; }
    public float CriticalChance { get => m_criticalChance; }
    public int Knockback { get => m_knockback; }
    public DamageType Type { get => m_type; }
    public float Duration { get => m_duration; }
}
