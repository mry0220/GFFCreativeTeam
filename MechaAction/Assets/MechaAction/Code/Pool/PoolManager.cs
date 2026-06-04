using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class InitPoolData
    {
        public GameObject m_prefab;
        public int m_initialSize;
    }

    [SerializeField] private List<InitPoolData> m_poolList;

    private Dictionary<GameObject, Queue<GameObject>> m_pool
        = new Dictionary<GameObject, Queue<GameObject>>();

    [SerializeField] private int addSize;

    private void Awake()
    {
        foreach (var data in m_poolList)
        {
            CreatePool(data.m_prefab, data.m_initialSize);
        }
    }

    public GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (!m_pool.ContainsKey(prefab))
        {
            CreatePool(prefab, addSize);
        }

        GameObject obj;
        if (m_pool[prefab].Count > 0)
        {
            obj = m_pool[prefab].Dequeue();//front
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
        var objPool = obj.GetComponent<ObjectPool>();

        if (objPool == null)
        {
            Destroy(obj);
            return;
        }

        var prefab = objPool.m_prefab;

        obj.SetActive(false);
        m_pool[prefab].Enqueue(obj);
    }

    private void CreatePool(GameObject prefab, int initialSize)
    {
        var queue = new Queue<GameObject>();

        for (int i = 0; i < initialSize; i++)
        {
            var obj = CreateObject(prefab);
            obj.SetActive(false);
            queue.Enqueue(obj);//back
        }

        m_pool.Add(prefab, queue);

    }

    [SerializeField] private GameObject m_parent;

    private GameObject CreateObject(GameObject prefab)
    {
        var obj = Instantiate(prefab, m_parent.transform);

        var objPool = obj.GetComponent<ObjectPool>();
        if (objPool == null)
        {
            objPool = obj.AddComponent<ObjectPool>();
        }

        objPool.m_prefab = prefab;
        objPool.m_pool = this;


        return obj;
    }
}
