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
    public int knockback;

    public DamageType type;

    public float duration;
}

public struct DamageResult
{
    public Vector3 hitPoint;
    public Vector3 hitNormal;
    public Vector3 attackDir;

    public EffectDataSO overrideEffect;//特別なエフェクトを出したいとき
    public AudioDataSO overrideAudio;//特別なAudioを出したいとき
}

public struct ApplyDamageEvent
{
    public Vector3 hitPoint;
    public Vector3 hitNormal;
    public Vector3 attackDir;

    public EffectDataSO effect;//特別なエフェクトを出したいとき
    public AudioDataSO audio;//特別なAudioを出したいとき
}

public interface IDamage
{
    void TakeDamage(DamageData data,DamageResult result);
}
