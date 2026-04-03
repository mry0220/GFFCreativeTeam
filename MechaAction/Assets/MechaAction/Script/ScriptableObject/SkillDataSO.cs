using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "SctiptableObjects/Data/SkillData")]
public class SkillDataSO : ScriptableObject
{
    [SerializeField] private string m_name;
    [SerializeField] private int m_id;
    [SerializeField] private int m_cost;
    [SerializeField] private SkillDataSO[] m_needSkill;
    [SerializeField] private SkillType m_type;
    [SerializeField] private float m_value;

    public int ID { get => m_id; }
    public int Cost { get => m_cost; }
    public SkillDataSO[] NeedSkill { get => m_needSkill; }
    public SkillType Type { get => m_type; }
    public float Value { get => m_value; }
}
