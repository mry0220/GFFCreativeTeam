using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "SctiptableObjects/Events/BoolEvent")]
public class BoolEvent : ScriptableObject
{
    private event Action<bool> OnEvent;

    public void Raise(bool d_event)
    {
        OnEvent?.Invoke(d_event);
    }

    public void Register(Action<bool> d_event)
    {
        OnEvent += d_event;
    }

    public void Unregister(Action<bool> d_event)
    {
        OnEvent -= d_event;
    }
}
