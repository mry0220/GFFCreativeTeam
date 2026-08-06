using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObjectPool : ObjectPool
{
    //this code is attach effect
    //effect auto return pool

    [SerializeField] private float m_time;

    public override void Init()
    {
        base.Init();

        Initialized(m_time);
    }

    public void Initialized(float time)
    {
        Invoke(nameof(OnDespawn), time);
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }
}
