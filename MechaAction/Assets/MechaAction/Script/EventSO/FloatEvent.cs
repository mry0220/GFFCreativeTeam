using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "SctiptableObjects/Events/FloatEvent")]
public class FloatEvent : ScriptableObject
{
    private event Action<float> OnEvent;

    public void Raise(float d_event)
    {
        OnEvent?.Invoke(d_event);
    }

    public void Register(Action<float> d_event)
    {
        OnEvent += d_event;
    }

    public void Unregister(Action<float> d_event)
    {
        OnEvent -= d_event;
    }
}
