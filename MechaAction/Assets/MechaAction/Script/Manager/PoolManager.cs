using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Transform m_parent;

    private Dictionary<GameObject, Queue<GameObject>> m_pool
        = new Dictionary<GameObject, Queue<GameObject>>();

    [SerializeField] private int m_initialSize = 10;

    public GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (!m_pool.ContainsKey(prefab))
        {
            CreatePool(prefab);
        }

        var pool = m_pool[prefab];

        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();//êÊì™Ç©ÇÁéÊÇÈ
        }
        else
        {
            obj = CreateObject(prefab);
        }

        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.SetActive(true);

        return obj;
    }

    public void Return(GameObject obj)
    {
        var poolObj = obj.GetComponent<ObjectPool>();

        if (poolObj == null)
        {
            Destroy(obj);
            return;
        }

        var prefab = poolObj.m_prefab;

        obj.SetActive(false);
        m_pool[prefab].Enqueue(obj);//å„ÇÎÇ…í«â¡
    }

    private void CreatePool(GameObject prefab)
    {
        var queue = new Queue<GameObject>();

        for (int i = 0; i < m_initialSize; i++)
        {
            var obj = CreateObject(prefab);
            obj.SetActive(false);
            queue.Enqueue(obj);//å„ÇÎÇ…í«â¡
        }

        m_pool.Add(prefab, queue);
    }

    private GameObject CreateObject(GameObject prefab)
    {
        var obj = Instantiate(prefab, m_parent);

        var poolObj = obj.GetComponent<ObjectPool>();
        if (poolObj == null)
        {
            poolObj = obj.AddComponent<ObjectPool>();
        }

        poolObj.m_prefab = prefab;

        return obj;
    }
}
