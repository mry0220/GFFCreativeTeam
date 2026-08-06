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
    None,
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
    public static PoolPath Instance { get; private set; }

    //enemy pool path
    [SerializeField] private PoolManager m_compassPool;
    [SerializeField] private PoolManager m_dronePool;
    [SerializeField] private PoolManager m_eyePool;

    private Dictionary<EnemyKind, PoolManager> m_enemyPools = new();

    //effect pool path
    [SerializeField] private PoolManager m_normalHitPool;

    private Dictionary<EffectKind, PoolManager> m_effectPools = new();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializedEnemyPools();

        InitializedEffectPools();
    }

    private void InitializedEnemyPools()
    {
        if (m_compassPool != null)
        {
            m_enemyPools.Add(EnemyKind.Compass, m_compassPool);
        }

        if (m_dronePool != null)
        {
            m_enemyPools.Add(EnemyKind.Drone, m_dronePool);
        }

        if (m_eyePool != null)
        {
            m_enemyPools.Add(EnemyKind.Eye, m_eyePool);
        }
    }

    private void InitializedEffectPools()
    {
        if(m_normalHitPool != null)
        {
            m_effectPools.Add(EffectKind.NHit, m_normalHitPool);
        }
    }

    public void CallEnemyPoolObj(EnemyKind kind, Vector3 pos, Quaternion rot)
    {
        m_enemyPools[kind].Get(pos, rot);
    }

    public void CallEffectPoolObj(EffectKind kind, Vector3 pos, Quaternion rot)
    {
        if(kind == EffectKind.None) return;

        m_effectPools[kind].Get(pos, rot);
    }
}
