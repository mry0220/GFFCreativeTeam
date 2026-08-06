using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //[System.Serializable]
    //public class InitPoolData
    //{
    //    public GameObject m_prefab;
    //    public int m_initialSize;
    //}

    //[SerializeField] private List<InitPoolData> m_poolList;

    //private Dictionary<GameObject, Queue<GameObject>> m_pool
    //    = new Dictionary<GameObject, Queue<GameObject>>();

    private Queue<GameObject> m_pool = new();
    [SerializeField] private GameObject m_prefab;

    [SerializeField] private int m_initialSize = 10;

    private void Awake()
    {

        CreatePool(m_prefab, m_initialSize);

    }

    public GameObject Get(Vector3 pos, Quaternion rot)
    {
        GameObject obj;
        if (m_pool.Count > 0)
        {
            obj = m_pool.Dequeue();//front
        }
        else
        {
            obj = CreateObject(m_prefab);
        }

        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.SetActive(true);

        if(obj.TryGetComponent<ObjectPool>(out var pool))
        {
            pool.OnSpawn();
        }

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

        obj.SetActive(false);
        m_pool.Enqueue(obj);
    }

    private void CreatePool(GameObject prefab, int initialSize)
    {
        for (int i = 0; i < initialSize; i++)
        {
            var obj = CreateObject(prefab);
            obj.SetActive(false);
            m_pool.Enqueue(obj);//back
        }

    }

    [SerializeField] private GameObject m_parent;

    private GameObject CreateObject(GameObject prefab)
    {
        var obj = Instantiate(prefab, m_parent.transform);

        var objPool = obj.GetComponent<ObjectPool>();
        if (objPool == null)
        {
            //objPool = obj.AddComponent<ObjectPool>();
            Debug.LogError("Don`t forget to attack ObjectPool");
        }

        objPool.m_pool = this;


        return obj;
    }
}
