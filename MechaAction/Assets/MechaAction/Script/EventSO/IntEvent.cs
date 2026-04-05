using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "SctiptableObjects/Events/IntEvent")]
public class IntEvent : ScriptableObject
{
    private event Action<int> OnEvent;

    public void Raise(int d_event)
    {
        OnEvent?.Invoke(d_event);
    }

    public void Register(Action<int> d_event)
    {
        OnEvent += d_event;
    }

    public void Unregister(Action<int> d_event)
    {
        OnEvent -= d_event;
    }
}
