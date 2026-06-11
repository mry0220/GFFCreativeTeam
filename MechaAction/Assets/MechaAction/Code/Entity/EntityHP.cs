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
    public AttackType Type;
    public float Damage;
    public float CriticalRate;
    public float CriticalChance;
    public float KnockBack;
    public float Duration;

    public EffectDataSO OverrideEffect;//special audio
    public AudioDataSO OverrideAudio;//special audio
}

public struct DamageResult
{
    public Vector3 HitPoint;
    public Vector3 HitNormal;
    public Vector3 AttackDir;
    public Vector3 KnockBackDir;
}

public struct EffectEvent
{
    public Vector3 HitPoint;
    public Vector3 HitNormal;

    public EffectDataSO EffectData;
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
    public void OnInitialized()
    {
        m_maxHP = m_entity.MaxHP;
        m_currentHP = m_maxHP;
    }

    public void OnTakeDamage(DamageData data, DamageResult result)
    {
        bool IsCritical = Critical(data.CriticalChance);

        switch(data.Type)
        {
            case AttackType.Normal:
                OnDamage(data, IsCritical);
                break;
            case AttackType.Electric:

                break;
            case AttackType.Ban:

                break;
            case AttackType.Heal:

                break;
        }

        EffectEvent effectData = new EffectEvent
        {
            HitPoint = result.HitPoint,
            HitNormal = result.HitNormal,
        };
    }

    private bool Critical(float chance)
    {
        float crit = UnityEngine.Random.Range(1f, 100f);
        bool IsCritical = crit < chance ? true : false;
        
        return IsCritical;
    }


    private void OnDamage(DamageData data, bool IsCritical)
    {
        if(IsCritical)
        {
            int value = (int)(data.Damage * data.CriticalRate);
            m_currentHP -= value;

            //damageUI
            //audio
        }
        else
        {
            m_currentHP -= data.Damage;

            //damageUI
            //audio
        }
    }

    private void OnKnockBack()
    {
        
    }

    private void OnElectBuff()
    {

    }

    private void OnStun()
    {

    }

    private void OnHeal()
    {

    }



    protected abstract void OnDead();
}
