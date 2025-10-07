using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("スポーンする敵プレハブ")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("プレイヤーとの距離設定")]
    [SerializeField] private float spawnDistance = 10f;   // この距離以内に来たら出現
    [SerializeField] private float despawnDistance = 15f; // この距離より離れたら削除

    private GameObject currentEnemy;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;

        // GManagerなどからプレイヤーTransformを取得（直接FindしてもOK）
        if (GManager.Instance != null)
        {
            //player = GManager.Instance.player.transform;
        }
        else
        {
            
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // プレイヤーが近づいたらスポーン
        if (distance < spawnDistance && currentEnemy == null)
        {
            Debug.Log("spawn");
            SpawnEnemy();
        }

        // プレイヤーが離れたら消滅
        if (currentEnemy != null && distance > despawnDistance)
        {
            Debug.Log("despawn");
            DespawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        currentEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }

    private void DespawnEnemy()
    {
        if (currentEnemy != null)
        {
            Destroy(currentEnemy);
            currentEnemy = null;
        }
    }
}
