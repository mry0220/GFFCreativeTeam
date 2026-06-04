using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoReturn : MonoBehaviour
{
    //this code is attach effect
    //effect auto return pool

    private ObjectPool m_objPool;

    private float m_time;

    public void Init(float time)
    {
        m_time = time;

        m_objPool = GetComponent<ObjectPool>();

        Invoke(nameof(Return), m_time);
    }

    private void Return()
    {
        m_objPool.Return();
    }
}
