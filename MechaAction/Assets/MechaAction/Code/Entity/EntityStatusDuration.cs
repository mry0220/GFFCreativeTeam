using System.Collections.Generic;
using UnityEngine;

public class StatusInstance
{
    public float m_duration;

    public EntityStatus m_status;

    public StatusModifier m_modifier;
}

public class EntityStatusDuration : MonoBehaviour
{
    private Entity m_Entity;

    private List<StatusInstance> m_buffs;

    private void Awake()
    {
        m_Entity = GetComponent<Entity>();
    }

    public void AddBuff(StatusModifier modifier, float duration)
    {
        EntityStatus status = m_Entity.GetStatus(modifier.m_statType);

        StatusInstance instance = new StatusInstance
        {
            m_duration = duration,
            m_status = status,
            m_modifier = modifier,
        };

        m_buffs.Add(instance);

        status.AddModifier(modifier);
    }

    private void Update()
    {
        for (int i = m_buffs.Count - 1; i >= 0; i--)
        {
            var buff = m_buffs[i];

            buff.m_duration -= Time.deltaTime;

            if (buff.m_duration <= 0)
            {
                RemoveBuff(buff);

                m_buffs.RemoveAt(i);
            }
        }
    }

    public void RemoveBuff(StatusInstance buff)
    {
        //buff.m_status.RemoveModifier(buff.m_modifier);
    }
}
