using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public PoolManager m_pool;

    public virtual void Init() { }
    public virtual void OnSpawn() { }
    public virtual void OnDespawn() 
    {
        Return();
    }

    private void Return()
    {
        m_pool.Return(gameObject);
    }
}
