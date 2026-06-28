using UnityEngine;
using System.Collections.Generic;

public class AreaSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnEnemy
    {
        public EnemyKind m_enemyKind;

        public Transform m_pos;
    }

    [System.Serializable]
    public class Wave
    {
        public List<SpawnEnemy> m_enemies = new();
    }

    [SerializeField] private List<Wave> m_waves = new();

    private int m_waveIndex = 0;

    private int m_aliveEnemyCount;

    public void OnWaveStart()
    {
        m_waveIndex = 0;
        SpawnWave(m_waveIndex);
    }

    private void SpawnWave(int index)
    {
        if(index >= m_waveIndex)
        {
            //finish
            return;
        }

        //spawnEnemies recode count
        m_aliveEnemyCount = m_waves[index].m_enemies.Count;

        foreach(var enemy in m_waves[index].m_enemies)
        {
            //m_enemies type  pool get enemy
            //this enemy spawn pos;
            //enemy = obj.GetComponent<EnemyHP>();
            //enemy.m_OnEnemyDied += OnEnemyDied;
        }
    }

    //if all spawnEnemy died, next wave
    //private void OnEnemyDied(EnemyHP enemy)
    //{
    //    //enemy.m_OnEnemyDied -= OnEnemyDied;

    //    m_aliveEnemyCount--;

    //    if(m_aliveEnemyCount <= 0)
    //    {
    //        m_waveIndex++;
    //        SpawnWave(m_waveIndex);
    //    }
    //}

    //この方法だとプレイヤーが死んで戻って　リセットする際
    //エネミーのEventを消せるかあやしい　
}
