using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;

    [SerializeField] private float spawnDistance = 10f;   // この距離以内に来たら出現
    [SerializeField] private float despawnDistance = 15f; // この距離より離れたら削除

    private GameObject _currentEnemy;
    private Transform _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player")?.transform;

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
        if (_player == null) return;

        float distance = Vector3.Distance(transform.position, _player.position);

        // プレイヤーが近づいたらスポーン
        if (distance < spawnDistance && _currentEnemy == null)
        {
            Debug.Log("spawn");
            SpawnEnemy();
        }

        // プレイヤーが離れたら消滅
        if (distance > despawnDistance && _currentEnemy != null)
        {
            Debug.Log("despawn");
            DespawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        _currentEnemy = Instantiate(_enemy, transform.position, Quaternion.identity);
    }

    private void DespawnEnemy()
    {
        if (_currentEnemy != null)
        {
            Destroy(_currentEnemy);
            _currentEnemy = null;
        }
    }
}
