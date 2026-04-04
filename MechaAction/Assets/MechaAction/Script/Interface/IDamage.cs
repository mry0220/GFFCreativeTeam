using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
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

    public DamageType type;

    public float duration;
}

public struct DamageResult
{
    public Vector3 hitPoint;
    public Vector3 hitNormal;

    public EffectDataSO overrideEffectData;//特別なエフェクトを出したいとき
    public AudioDataSO overrideAudioData;//特別なAudioを出したいとき
}

public struct ApplyDamageEvent
{
    public Vector3 hitPoint;
    public Vector3 hitNormal;

    public EffectDataSO effectData;
    public AudioDataSO audioData;
}

public interface IDamage
{
    void TakeDamage(DamageData data,DamageResult result);
}
