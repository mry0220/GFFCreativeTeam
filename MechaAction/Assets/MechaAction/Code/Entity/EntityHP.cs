using UnityEngine;

public abstract class EntityHP : MonoBehaviour
{
    //StateValue--------------------
    private float m_maxHP;
    private float m_currentHP;

    public float CurrentHP { get => m_currentHP; }
    //------------------------------


    protected void Awake()
    {
        
    }

    //call frome Entity, give hp data
    public void OnInitialized(EntityDataSO data)
    {
        m_maxHP = data.MaxHP;
        m_currentHP = m_maxHP;
    }

    public void OnTakeDamage()
    {

    }

    protected abstract void OnDead();
}
