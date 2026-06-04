using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject m_prefab;

    public PoolManager m_pool;

    public void Return()
    {
        m_pool.Return(gameObject);
    }
}
