using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject m_enemy;

    [SerializeField] private float _spawnDistance;   // この距離以内に来たら出現
    [SerializeField] private float _respawnDistance; // この距離より離れたら削除
    private bool m_canSpawn = true;

    [Header("Pool")]
    [SerializeField] private PoolManager m_pool;

    [Header("Event")]
    [SerializeField] private EventSO m_enemySpawnReset;

    private GameObject m_currentEnemy;
    private Transform m_player;

    private void OnEnable()
    {
        m_enemySpawnReset.Register(SpawnClear);
    }

    private void OnDisable()
    {
        m_enemySpawnReset.Unregister(SpawnClear);
    }

    private void Awake()
    {
        m_player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (m_player == null) return;

        float distance = Vector3.Distance(transform.position, m_player.position);

        if(m_currentEnemy != null) return;

        // プレイヤーが近づいたらスポーン
        if (distance <= _spawnDistance && m_canSpawn)
        {
            SpawnEnemy();
            m_canSpawn = false;
        }
        else if (distance > _respawnDistance)
        {
            m_canSpawn = true;

        }
    }

    private void SpawnEnemy()
    {
        //m_currentEnemy = m_pool.Get(
        //        m_enemy,
        //        transform.position,
        //        Quaternion.identity
        //    );
        var enemyInfo = m_currentEnemy.GetComponent<EnemyHP>();
        enemyInfo.m_OnEnemyDied += OnEnemyDied;
        enemyInfo.Init(m_pool);
    }

    public void SpawnClear()
    {
        m_currentEnemy = null;
        m_canSpawn = true;
    }

    private void OnEnemyDied(EnemyHP enemy)
    {
        enemy.m_OnEnemyDied -= OnEnemyDied;

        if (m_currentEnemy  != null)
        {
            m_currentEnemy = null;
        }
    }
}
