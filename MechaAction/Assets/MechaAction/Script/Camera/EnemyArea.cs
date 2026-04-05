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
    private bool spawning = false;

    private int m_aliveCount;

    public void StartSpawn()
    {
        if (spawning) return;
        spawning = true;
        currentWaveIndex = 0;
        SpawnWave(currentWaveIndex);
    }

    private void SpawnWave(int waveIndex)
    {
        if (!spawning) return;

        if (waveIndex >= waves.Count)
        {
            Debug.Log("All wave finish");
            Clear();
            spawning = false;
            return;
        }

        m_aliveCount = waves[waveIndex].enemies.Count;

        Debug.Log($"Wave {waveIndex + 1} Start");

        foreach (var enemyData in waves[waveIndex].enemies)
        {
            GameObject enemy = Instantiate(enemyData.enemyPrefab, enemyData.spawnPoint.position, Quaternion.identity);
            enemy.GetComponent<EnemyHP>().m_OnEnemyDied += OnEnemyDied;
        }
    }

    private void OnEnemyDied(EnemyHP enemy)
    {
        enemy.m_OnEnemyDied -= OnEnemyDied;

        m_aliveCount--;

        if (m_aliveCount <= 0)
        {
            currentWaveIndex++;
            SpawnWave(currentWaveIndex);
        }
    }

}
