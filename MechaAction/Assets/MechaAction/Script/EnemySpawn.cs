using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;

    [SerializeField] private float _spawnDistance = 10f;   // この距離以内に来たら出現
    [SerializeField] private float _respawnDistance = 15f; // この距離より離れたら削除
    private bool m_canSpawn = true;

    private GameObject m_currentEnemy;
    private Transform m_player;

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
        m_currentEnemy = Instantiate(_enemy, transform.position, Quaternion.identity);
    }

    public void DeadClear()
    {
        m_currentEnemy = null;
    }

    private void DespawnEnemy()
    {
        if (m_currentEnemy != null)
        {
            Destroy(m_currentEnemy);
            m_currentEnemy = null;
        }
    }
}
