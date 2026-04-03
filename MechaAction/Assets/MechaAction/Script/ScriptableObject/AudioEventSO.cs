using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "SctiptableObjects/Events/AudioEvent")]
public class AudioEventSO : ScriptableObject
{
    public event Action<AudioDataSO> OnEvent;

    public void Raise(AudioDataSO d_event)
    {
        OnEvent?.Invoke(d_event);
    }

    public void Register(Action<AudioDataSO> d_event)
    {
        OnEvent += d_event;
    }

    public void Unregister(Action<AudioDataSO> d_event)
    {
        OnEvent -= d_event;
    }
}
