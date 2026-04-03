using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "SctiptableObjects/Events/SkillEvent")]
public class SkillEventSO : ScriptableObject
{
    public event Action<SkillDataSO> OnEvent;

    public void Raise(SkillDataSO d_event)
    {
        OnEvent?.Invoke(d_event);
    }

    public void Register(Action<SkillDataSO> d_event)
    {
        OnEvent += d_event;
    }

    public void Unregister(Action<SkillDataSO> d_event)
    {
        OnEvent -= d_event;
    }
}
