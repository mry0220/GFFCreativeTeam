using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Event", menuName = "SctiptableObjects/Events/Event")]
public class EventSO : ScriptableObject
{
    public event Action OnEvent;

    public void Raise()
    {
        OnEvent?.Invoke();
    }

    public void Register(Action d_event)
    {
        OnEvent += d_event;
    }

    public void Unregister(Action d_event)
    {
        OnEvent -= d_event;
    }
}
