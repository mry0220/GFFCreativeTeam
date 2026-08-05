using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArea : MonoBehaviour
{
    [SerializeField] private CameraArea m_cameraArea;
    [SerializeField] private GameObject m_wall;

    public void Activate()
    {
        m_wall.SetActive(true);
        m_cameraArea.gameObject.SetActive(true);
        StartSpawn();
    }

    public void Clear()
    {
        m_wall.SetActive(false);
        m_cameraArea.gameObject.SetActive(false);
    }

    //================== â∫ãLÅ@EnemyAreaSpawn ==================

    [System.Serializable]
    public class EnemySpawnData
    {
        [Header("Enemy")]
        public GameObject enemyPrefab;

        [Header("Pos")]
        public Transform spawnPoint;
    }

    [System.Serializable]
    public class Wave
    {
        [Header("Enemies")]
        public List<EnemySpawnData> enemies = new List<EnemySpawnData>();
    }

    [Header("Wave")]
    [SerializeField] private List<Wave> waves = new List<Wave>();

    private int currentWaveIndex = 0;
    private bool m_spawning = false;

    [SerializeField] private PoolManager m_pool;
    private int m_aliveCount;

    [Header("Event")]
    [SerializeField] private EventSO m_enemySpawnReset;

    private void OnEnable()
    {
        m_enemySpawnReset.Register(AreaSpawnClear);
    }

    private void OnDisable()
    {
        m_enemySpawnReset.Unregister(AreaSpawnClear);
    }

    public void StartSpawn()
    {
        if (m_spawning) return;
        m_spawning = true;
        currentWaveIndex = 0;
        SpawnWave(currentWaveIndex);
    }

    private void SpawnWave(int waveIndex)
    {
        if (!m_spawning) return;

        if (waveIndex >= waves.Count)
        {
            Debug.Log("All wave finish");
            Clear();
            m_spawning = false;
            return;
        }

        m_aliveCount = waves[waveIndex].enemies.Count;

        Debug.Log($"Wave {waveIndex + 1} Start");

        foreach (var enemyData in waves[waveIndex].enemies)
        {
            //GameObject enemy = m_pool.Get(
            //    enemyData.enemyPrefab, 
            //    enemyData.spawnPoint.position, 
            //    Quaternion.identity
            //);
            //var enemyInfo = enemy.GetComponent<EnemyHP>();
            //enemyInfo.m_OnEnemyDied += OnEnemyDied;
            //enemyInfo.Init(m_pool);
        }
    }

    private void OnEnemyDied(EnemyHPT enemy)
    {
        enemy.m_OnEnemyDied -= OnEnemyDied;

        m_aliveCount--;

        if (m_aliveCount <= 0)
        {
            currentWaveIndex++;
            SpawnWave(currentWaveIndex);
        }
    }

    private void AreaSpawnClear()
    {
        m_spawning = false;
    }
}
