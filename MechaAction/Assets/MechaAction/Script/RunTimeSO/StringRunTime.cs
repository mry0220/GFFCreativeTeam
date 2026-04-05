using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RunTime", menuName = "SctiptableObjects/RunTime/String")]
public class StringRunTime : ScriptableObject
{
    [SerializeField] private string m_value;

    public string Value { get => m_value; }

    public void SetValue(string value)
    {
        m_value = value;
    }
}
