using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private DamageEventSO m_DamageEventSO;

    [SerializeField] private PoolManager m_pool;

    private void OnEnable()
    {
        m_DamageEventSO.Register(InstantiateEffect);
    }

    private void OnDisable()
    {
        m_DamageEventSO.Unregister(InstantiateEffect);
    }

    public void InstantiateEffect(EffectEvent d_event)
    {
        if (d_event.effectData == null) return;

        var rot = Quaternion.LookRotation(d_event.hitNormal);

        var effect = m_pool.Get(
            d_event.effectData.EffectPrefab,
            d_event.hitPoint,
            rot
        );

        //effect.GetComponent<AutoReturn>().Init(
        //    m_pool, 
        //    d_event.effectData.Duration
        //);
    }
}
