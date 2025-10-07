using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEnemySpawn : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        [Header("出す敵")]
        public GameObject enemyPrefab;

        [Header("出す位置")]
        public Transform spawnPoint;
    }

    [System.Serializable]
    public class Wave
    {
        [Header("このWaveで出す敵たち")]
        public List<EnemySpawnData> enemies = new List<EnemySpawnData>();
    }

    [Header("Wave設定")]
    [SerializeField] private List<Wave> waves = new List<Wave>();

    private int currentWaveIndex = 0;
    private List<GameObject> aliveEnemies = new List<GameObject>();
    private bool spawning = false;

    // Cameralimitから呼び出して最初のWaveを開始
    public void StartSpawn()
    {
        if (spawning) return;
        spawning = true;
        currentWaveIndex = 0;
        SpawnWave(currentWaveIndex);
    }

    // Waveを生成
    private void SpawnWave(int waveIndex)
    {
        if (waveIndex >= waves.Count)
        {
            Debug.Log("全てのWaveが終了しました！");
            // Cameralimitに通知してエリア解放
            Cameralimit limit = GetComponent<Cameralimit>();
            //if (limit != null) limit.OnEnemiesCleared();
            return;
        }

        Debug.Log($"Wave {waveIndex + 1} 開始");

        aliveEnemies.Clear();

        foreach (var enemyData in waves[waveIndex].enemies)
        {
            GameObject enemy = Instantiate(enemyData.enemyPrefab, enemyData.spawnPoint.position, Quaternion.identity);
            aliveEnemies.Add(enemy);

            // 敵死亡時のイベント登録
            Enemy e = enemy.GetComponent<Enemy>();
            if (e != null)
            {
                //e.onDeath += () => OnEnemyDied(enemy);
            }
        }
    }

    // 敵死亡時の処理
    private void OnEnemyDied(GameObject enemy)
    {
        aliveEnemies.Remove(enemy);

        // Wave全滅 → 次のWaveへ
        if (aliveEnemies.Count == 0)
        {
            currentWaveIndex++;
            SpawnWave(currentWaveIndex);
        }
    }
}
