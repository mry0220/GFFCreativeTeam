using System.Collections.Generic;
using UnityEngine;

public enum PlayerAttackType
{
    NormalAttack,
    VoltSlash,
    GrandSlash,

    NormalGun,
    ShockShot,
    FullBurst
}

public class PlayerAttack : MonoBehaviour
{

    //attackData----------------
    [Header("NormalAttack")]
    [SerializeField] private AttackDataSO m_normalAttackData;
    [SerializeField] private HitCollider m_normalCollider;
    [Header("VoltSlash")]
    [SerializeField] private AttackDataSO m_voltSlashData;
    [SerializeField] private HitCollider m_voltSlashCollider;
    [Header("GrandSlash")]
    [SerializeField] private AttackDataSO m_grandSlashData;
    [SerializeField] private HitCollider m_grandSlashCollider;

    [Header("NormalGun")]
    [SerializeField] private AttackDataSO m_normalGunData;
    [SerializeField] private HitRay m_normalGunCollider;

    //DamageDatalist, attackData is makeing damageData early
    private Dictionary<PlayerAttackType, DamageData> m_damageDataList = new();

    private void Start()
    {
        m_damageDataList.Add(PlayerAttackType.NormalAttack, OnGetDamageData(m_normalAttackData));
        m_damageDataList.Add(PlayerAttackType.VoltSlash,        OnGetDamageData(m_voltSlashData));
        m_damageDataList.Add(PlayerAttackType.GrandSlash, OnGetDamageData(m_grandSlashData));

        m_damageDataList.Add(PlayerAttackType.NormalGun, OnGetDamageData(m_normalGunData));
    }

    private DamageData OnGetDamageData(AttackDataSO dataSO)
    {
        DamageData data = new DamageData
        {
            Type = dataSO.Type,
            Damage = dataSO.Damage,
            CriticalRate = dataSO.CriticalRate,
            CriticalChance = dataSO.CriticalChance,
            KnockBack = dataSO.Knockback,
            Duration = dataSO.Duration,
            OverrideEffect = dataSO.Effect,
            OverrideAudio = dataSO.Audio,
        };

        return data;
    }

    public void OnAttack(PlayerAttackType type, TeamType team, Vector3 forward)
    {
        var data = m_damageDataList[type];

        switch (type)
        {
            case PlayerAttackType.NormalAttack:
                m_normalCollider.AttackCollider(data, team, forward);
                break;
            case PlayerAttackType.VoltSlash:
                m_voltSlashCollider.AttackCollider(data, team, forward);
                break;
            case PlayerAttackType.GrandSlash:
                m_grandSlashCollider.AttackCollider(data, team, forward);
                break;
            case PlayerAttackType.NormalGun:
                m_normalGunCollider.AttackCastPenetration(data, team, forward);
                break;
        }
    }
}
