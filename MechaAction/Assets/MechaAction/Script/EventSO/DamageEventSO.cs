using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "SctiptableObjects/Events/DamageEvent")]
public class DamageEventSO : ScriptableObject
{
    public event Action<ApplyDamageEvent> OnDamaged;

    public void Raise(ApplyDamageEvent d_event)
    {
        OnDamaged?.Invoke(d_event);
    }

    public void Register(Action<ApplyDamageEvent> d_event)
    {
        OnDamaged += d_event;
    }

    public void Unregister(Action<ApplyDamageEvent> d_event)
    {
        OnDamaged -= d_event;
    }

}
