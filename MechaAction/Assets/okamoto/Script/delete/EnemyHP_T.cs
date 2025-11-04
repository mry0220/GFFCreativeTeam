using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyHP_T : MonoBehaviour, IDamage
{
    [SerializeField] private float maxHP = 50f; // HP

    private Transform _player;
    private Rigidbody _rb;
    private float currentHP;
    //[SerializeField] private float currentHealth; // デバッグ用にInspectorで確認可能になる

    // IHealthインターフェースの実装（読み取り専用プロパティ）
    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    public GameObject _damageeffect;

    void Start()
    {
        currentHP = maxHP;
        Debug.Log(gameObject.name + " (敵) のHPが初期化されました: " + currentHP);
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        DistanceDead();
    }

    //ダメージを受け、HPを減少させる処理
    public void TakeDamage(int amount,int knockback,int dir)
    {
        if (currentHP <= 0) return;

        currentHP -= amount;
        currentHP = Mathf.Max(currentHP, 0); // 0未満にならないようにクランプ

        //_damageeffect = 
        GameObject effect = Instantiate(_damageeffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.2f); // アニメーションの長さに合わせて

        if (knockback != 0)
        {
            _rb.AddForce(dir * knockback, 3f, 0, ForceMode.Impulse);
        }

        Debug.Log(gameObject.name + " (敵) が" + amount + "ダメージ受けました。残りHP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    // HPを回復させる処理 (敵にはあまり使わないかもしれないが、インターフェースの契約として実装)
    public void Heal(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Min(currentHP, maxHP); // 最大HPを超えないようにクランプ

        Debug.Log(gameObject.name + " (敵) のHPが" + amount + "回復しました。残りHP: " + currentHP);
    }


    private void DistanceDead()
    {
        if (Vector3.Distance(transform.position, _player.position) > 30f)
        {
            Destroy(gameObject);
        }
    }

    // 敵独自の死亡処理
    public void Die()
    {
        Debug.Log(gameObject.name + " (敵) は倒されました！スコア加算と破壊を実行します。");

        // 独自の敵の死亡処理を記述
        // 例1: スコアを加算する処理
        // GameManager.Instance.AddScore(100); 

        // 例2: 爆発エフェクトを再生する処理
        // Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // 例3: オブジェクトをシーンから破壊
        Destroy(gameObject);
    }
}
