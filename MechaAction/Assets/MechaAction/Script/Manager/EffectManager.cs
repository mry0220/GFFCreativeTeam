using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private DamageEventSO m_DamageEventSO;

    private void OnEnable()
    {
        m_DamageEventSO.Register(InstantiateEffect);
    }

    private void OnDisable()
    {
        m_DamageEventSO.Unregister(InstantiateEffect);
    }

    public void InstantiateEffect(ApplyDamageEvent d_event)
    {
        if (d_event.effect == null) return;

        var rot = Quaternion.LookRotation(d_event.hitNormal);

        Instantiate(d_event.effect.EffectPrefab, d_event.hitPoint, rot);


    }

    public void PlayEffect()
    {

    }
}
