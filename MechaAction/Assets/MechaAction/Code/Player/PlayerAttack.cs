using System.Collections.Generic;
using UnityEngine;

public enum PlayerAttackType
{
    NormalAttack,
    Slash,
    GroundAttack,
    NormalGun,
    ShotGun,
    Rifle
}

public class PlayerAttack : MonoBehaviour
{
    //component-----------------
    private Entity m_entity;
    //--------------------------

    //attackData----------------
    [Header("NormalAttack")]
    [SerializeField] private AttackDataSO m_normalAttackData;
    [SerializeField] private HitCollider m_normalCollider;
    [Header("Slash")]
    [SerializeField] private AttackDataSO m_slashData;
    [SerializeField] private HitCollider m_slashCollider;

    //DamageDatalist, attackData is makeing damageData early
    private Dictionary<PlayerAttackType, DamageData> m_damageDataList = new();

    private void Awake()
    {
        m_entity = GetComponent<Entity>();
    }

    private void Start()
    {
        m_damageDataList.Add(PlayerAttackType.NormalAttack, OnGetDamageData(m_normalAttackData));
        m_damageDataList.Add(PlayerAttackType.Slash,        OnGetDamageData(m_slashData));
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

    public void OnAttack(PlayerAttackType type)
    {
        var data = m_damageDataList[type];

        switch (type)
        {
            case PlayerAttackType.NormalAttack:
                m_normalCollider.AttackCollider(data, m_entity.Team, m_entity.Forward);
                break;
            case PlayerAttackType.Slash:
                m_slashCollider.AttackCollider(data, m_entity.Team, m_entity.Forward);
                break;
        }
    }
}
