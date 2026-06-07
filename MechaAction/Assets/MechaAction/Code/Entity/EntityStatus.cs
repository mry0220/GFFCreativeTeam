using UnityEngine;
using System.Collections.Generic;

public enum ModifierType
{
    Add,
    Multiply
}

public class StatusModifier
{
    public StatusType m_statType;

    public float m_value;
    public ModifierType m_modType;
}

public class EntityStatus
{
    private float m_baseValue;

    private List<StatusModifier> m_modifier = new();

    public float Value
    {
        get
        {
            float add = 0;
            float multiply = 1;


            foreach(var modifier in m_modifier)
            {
                switch(modifier.m_modType)
                {
                    case ModifierType.Add:
                        add += modifier.m_value;
                        break;
                    case ModifierType.Multiply:
                        multiply += modifier.m_value;
                        break;
                }
            }

            return (m_baseValue + add) * multiply;
        }
    }

    public EntityStatus(float baseValue)
    {
        m_baseValue = baseValue;
    }

    public void AddModifier(StatusModifier modifier)
    {
        m_modifier.Add(modifier);
    }
}
