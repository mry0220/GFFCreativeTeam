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

    //public EffectDataSO OverrideEffect;//special audio
    public EffectKind OverrideEffect;
    public AudioDataSO OverrideAudio;//special audio
}

public struct DamageResult
{
    public Vector3 HitPoint;
    public Quaternion HitRot;
    public Vector3 AttackDir;
    public Vector3 KnockBackDir;
}

public struct EffectEvent
{
    public Vector3 HitPoint;
    public Quaternion HitRot;

    public EffectKind EffectKind;
}

public class EntityHP
{
    //component---------------------
    //private Entity m_entity;

    //StateValue--------------------
    //private float m_maxHP;
    //private float m_currentHP;

    //public float CurrentHP { get => m_currentHP; }
    //------------------------------

    private EffectKind m_hitEffect;

    private AudioDataSO m_hitAudio;


    //call frome Entity, give hp data
    public void OnInitialized(EffectKind kind, AudioDataSO audio)
    {
        m_hitEffect = kind;
        m_hitAudio = audio;
    }

    public float OnTakeDamage(float hp, DamageData data, DamageResult result)
    {
        bool IsCritical = Critical(data.CriticalChance);

        switch(data.Type)
        {
            case AttackType.Normal:
                hp = OnDamage(hp, data, IsCritical);
                break;
            case AttackType.Electric:

                break;
            case AttackType.Ban:

                break;
            case AttackType.Heal:

                break;
        }

        EffectEvent effectData;
        

        if(data.OverrideEffect == EffectKind.None)
        {
            effectData = new EffectEvent
            {
                HitPoint = result.HitPoint,
                HitRot = result.HitRot,
                EffectKind = m_hitEffect,
            };
        }
        else
        {
            effectData = new EffectEvent
            {
                HitPoint = result.HitPoint,
                HitRot = result.HitRot,
                EffectKind = data.OverrideEffect,
            };
        }

        PoolPath.Instance.CallEffectPoolObj(effectData);


        return hp;
    }

    private bool Critical(float chance)
    {
        float crit = UnityEngine.Random.Range(1f, 100f);
        bool IsCritical = crit < chance ? true : false;
        
        return IsCritical;
    }


    private float OnDamage(float hp, DamageData data, bool IsCritical)
    {
        if(IsCritical)
        {
            int value = (int)(data.Damage * data.CriticalRate);
            hp -= value;

            //damageUI
            //audio
        }
        else
        {
            hp -= data.Damage;

            //damageUI
            //audio
        }

        return hp;
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

}
