using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoReturn : MonoBehaviour
{
    private float m_lifeTime;

    private PoolManager m_pool;

    public void Init(PoolManager pool,float time)
    {
        m_pool = pool;
        m_lifeTime = time;

        Invoke(nameof(Return), m_lifeTime);
    }

    private void Return()
    {
        m_pool.Return(gameObject);
    }
}
