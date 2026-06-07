using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossSpawn : MonoBehaviour
{

    [Header("出す敵")]
    public GameObject enemyPrefab;

    [Header("出す位置")]
    public Transform spawnPoint;


    //[System.Serializable]
    //public class Wave
    //{
    //    [Header("このWaveで出す敵たち")]
    //    public List<EnemySpawnData> enemies = new List<EnemySpawnData>();
    //}

    //[Header("Wave設定")]
    //[SerializeField] private List<Wave> waves = new List<Wave>();

    //private int currentWaveIndex = 0;
    //private List<GameObject> aliveEnemies = new List<GameObject>();
    private bool spawning = false;
    private Cameralimit _limit;

    private GameObject enemy;

    [SerializeField] private Image hpBarImage;   // HP本体のImage
    [SerializeField] private GameObject hpBarObject;   // HP本体のImage
    private EnemyHP _enemyhp;

    private float maxHealth;
    private float currentHealth;


    private void Start()
    {
        hpBarObject.SetActive(false);
        _limit = GetComponent<Cameralimit>();
    }

    private void Update()
    {
        if(enemy != null)
        {
            currentHealth = _enemyhp.CurrentHP;
            UpdateBar();
        }
    }

    private void UpdateBar()
    {
        float percent = currentHealth / maxHealth;
        hpBarImage.fillAmount = percent;
    }

    // Cameralimitから呼び出して最初のWaveを開始
    public void StartSpawn()
    {
        // Debug.Log($"isSpawning : {spawning}");
        if (spawning) return;
        spawning = true;
        //Debug.Log($"Spawn");
        //currentWaveIndex = 0;
        //SpawnWave(currentWaveIndex);
        enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        if (enemy != null)
        {
            _enemyhp = enemy.GetComponent<EnemyHP>();
            hpBarObject.SetActive(true);
            maxHealth = _enemyhp.MaxHP;
            currentHealth = _enemyhp.CurrentHP;
            UpdateBar();
        }

        StartCoroutine(Wait());

        //while (enemy != null)
        //{

        //}
        //_limit.Clear();
    }

    private IEnumerator Wait()
    {
        // enemy が消えるまで待つ
        while (enemy != null)
        {
            yield return null; // ← 次のフレームへ
        }

        // 敵が死んだあと
        _limit.Clear();
        spawning = false;
        hpBarObject.SetActive(false );
        Debug.Log("ボス終了");
    }

    // Waveを生成
    //private void SpawnWave(int waveIndex)
    //{
    //    if (waveIndex >= waves.Count)
    //    {
    //        Debug.Log("全てのWaveが終了しました！");
    //        // Cameralimitに通知してエリア解放
    //        
    //        spawning = false;
    //        //if (limit != null) limit.OnEnemiesCleared();
    //        return;
    //    }

    //    Debug.Log($"Wave {waveIndex + 1} 開始");

    //    aliveEnemies.Clear();

    //    foreach (var enemyData in waves[waveIndex].enemies)
    //    {

    //    }
    //    StartCoroutine(WaitUntilAllDead());

    //}

    //private IEnumerator WaitUntilAllDead()
    //{
    //    while (aliveEnemies.Exists(enemy => enemy != null))
    //        yield return null;

    //    currentWaveIndex++;
    //    SpawnWave(currentWaveIndex);
    //}

    //// 敵死亡時の処理
    //private void OnEnemyDied(GameObject enemy)
    //{
    //    aliveEnemies.Remove(enemy);

    //    // Wave全滅 → 次のWaveへ
    //    if (aliveEnemies.Count == 0)
    //    {
    //        currentWaveIndex++;
    //        SpawnWave(currentWaveIndex);
    //    }
    //}
}
