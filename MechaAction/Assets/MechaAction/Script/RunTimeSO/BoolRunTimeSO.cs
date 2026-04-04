using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RunTime", menuName = "SctiptableObjects/RunTime/Bool")]
public class BoolRunTimeSO : ScriptableObject
{
    private bool m_value;

    public bool Value { get => m_value; }

    public void SetValue(bool value)
    {
        m_value = value;
    }
}
