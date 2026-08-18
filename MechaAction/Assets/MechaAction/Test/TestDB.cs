using UnityEngine;

[CreateAssetMenu(fileName = "TestDB", menuName = "Scriptable Objects/TestDB")]
public class TestDB : ScriptableObject
{
    [SerializeField] private int m_value;

    [SerializeField] private bool m_checked;

    //public int Value => m_value;

    //public bool Checked => m_checked;

    public void AddValue(int Value)
    {
        m_value += Value;
    }

    public void Toggle(bool Value)
    {
        m_checked = Value;
    }
}
