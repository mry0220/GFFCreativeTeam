using System.Collections.Generic;
using UnityEngine;

public enum DebugEnemyAttackType
{ 
    NormalAttack
}

public class DebugEnemyAttack : MonoBehaviour
{
    //attackData----------------
    [Header("NormalAttack")]
    [SerializeField] private AttackDataSO m_normalAttackData;
    [SerializeField] private HitCollider m_normalCollider;

    //DamageDatalist, attackData is makeing damageData early
    private Dictionary<DebugEnemyAttackType, DamageData> m_damageDataList = new();

    private void Start()
    {
        m_damageDataList.Add(DebugEnemyAttackType.NormalAttack, OnGetDamageData(m_normalAttackData));
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

    public void OnAttack(DebugEnemyAttackType type, TeamType team, Vector3 forward)
    {
        var data = m_damageDataList[type];

        switch (type)
        {
            case DebugEnemyAttackType.NormalAttack:
                m_normalCollider.AttackCollider(data, team, forward);
                break;
        }
    }
}
