using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackDataSO", menuName = "ScriptableObjects/Datas/AttackData")]
public class AttackDataSO : ScriptableObject
{
    [SerializeField] private AttackType m_type;
    [SerializeField] private float m_damage;
    [SerializeField] private float m_criticalRate;
    [SerializeField, Range(0f, 100f)] private float m_criticalChance;
    [SerializeField] private float m_knockback;
    [SerializeField] private float m_duration; //if attacktype elect, how long debuff
    [SerializeField] private EffectDataSO m_overrideEffect;
    [SerializeField] private AudioDataSO m_overrideAudio;

    public AttackType Type { get => m_type; }
    public float Damage { get => m_damage; }
    public float CriticalRate { get => m_criticalRate; }
    public float CriticalChance { get => m_criticalChance; }
    public float Knockback { get => m_knockback; }
    public float Duration {  get => m_duration; }
    public EffectDataSO Effect { get => m_overrideEffect; }
    public AudioDataSO Audio { get => m_overrideAudio; }
}
