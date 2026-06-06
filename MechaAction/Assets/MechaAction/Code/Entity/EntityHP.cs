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
    public int damage;
    public bool isCritical;
    public float criticalRate;
    public int knockback;
    public Vector3 attackDir;

    public AttackType type;

    public float duration;

    public EffectDataSO overrideEffectData;//特別なエフェクトを出したいとき
    public AudioDataSO overrideAudioData;//特別なAudioを出したいとき
}

public struct DamageResult
{
    public Vector3 hitPoint;
    public Vector3 hitNormal;

   
}

public struct ApplyDamageEvent
{
    public Vector3 hitPoint;
    public Vector3 hitNormal;

    public EffectDataSO effectData;
    public AudioDataSO audioData;
}

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
