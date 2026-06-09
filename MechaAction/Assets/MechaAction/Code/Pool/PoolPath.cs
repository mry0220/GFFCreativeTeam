using System.Collections.Generic;
using UnityEngine;

public enum EnemyKind
{
    Compass,
    Drone,
    Eye
}

public enum EffectKind
{
    NHit,
    CHit,
}

public enum UIKind
{
    NHit,
    CHit,
}

public class PoolPath : MonoBehaviour
{
    //enemy pool path
    [SerializeField] private PoolManager m_compassPool;
    [SerializeField] private PoolManager m_dronePool;
    [SerializeField] private PoolManager m_eyePool;

    private Dictionary<EnemyKind, PoolManager> m_enemyPools = new();

    private void Start()
    {
        m_enemyPools.Add(EnemyKind.Compass, m_compassPool);
        m_enemyPools.Add(EnemyKind.Drone, m_dronePool);
        m_enemyPools.Add(EnemyKind.Eye, m_eyePool);
    }

    public void CallEnemyPoolObj(EnemyKind kind, Vector3 pos, Quaternion rot)
    {
        m_enemyPools[kind].Get(pos, rot);
    }
}
