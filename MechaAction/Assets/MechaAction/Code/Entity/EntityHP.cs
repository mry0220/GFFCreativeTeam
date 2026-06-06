using UnityEngine;

public enum AttackType
{
    Normal,
    Electric,
    Ban,
    Heal
}

public struct DamageData
{
    public int Damage;
    public bool IsCritical;
    public float CriticalRate;
    public int KnockBack;

    public AttackType Type;

    public float Duration;

    public EffectDataSO OverrideEffect;//special audio
    public AudioDataSO OverrideAudio;//special audio

    public Vector3 AttackDir;

}

public struct DamageResult
{
    public Vector3 hitPoint;
    public Vector3 hitNormal;

   
}

public struct EffectEvent
{
    public Vector3 hitPoint;
    public Vector3 hitNormal;

    public EffectDataSO effectData;
}

public abstract class EntityHP : MonoBehaviour
{
    //component---------------------
    private Entity m_entity;

    //StateValue--------------------
    private float m_maxHP;
    private float m_currentHP;

    public float CurrentHP { get => m_currentHP; }
    //------------------------------


    protected virtual void Awake()
    {
        m_entity = GetComponent<Entity>();
    }

    //call frome Entity, give hp data
    public void OnInitialized(EntityDataSO data)
    {
        m_maxHP = data.MaxHP;
        m_currentHP = m_maxHP;
    }

    public void OnTakeDamage(DamageData data, DamageResult result)
    {
        switch(data.Type)
        {
            case AttackType.Normal:

                break;
        }

        EffectEvent effectData = new EffectEvent
        {

        };
    }

    private bool Critical(float chance)
    {
        float crit = UnityEngine.Random.Range(1f, 100f);
        bool IsCritical = crit < chance ? true : false;
        
        return IsCritical;
    }


    private void OnDamage()
    {

    }

    private void OnKnockBack()
    {
        
    }

    protected abstract void OnDead();
}
