using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour,IDamage
{
    [SerializeField] private float maxHP = 50f; // HP
    [SerializeField] DamageEffectSO _damageEffectSO;
    private IEnemy _ienemy;

    private Transform _player;
    private float currentHP;
    //[SerializeField] private float currentHealth; // デバッグ用にInspectorで確認可能になる

    // IHealthインターフェースの実装（読み取り専用プロパティ）
    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    void Start()
    {
        currentHP = maxHP;
        //Debug.Log("<color=red>" + gameObject.name + " (敵) のHPが初期化されました: " + currentHP);
        _player = GameObject.FindWithTag("Player").transform;
        _ienemy = GetComponent<IEnemy>();
    }

    private void Update()
    {
        DistanceDead();
    }

    //ダメージを受け、HPを減少させる処理
    public void TakeDamage(int damage, int knockback, int dir, string audioname)
    {
        if (currentHP <= 0) return;

        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0); // 0未満にならないようにクランプ
        GManager.Instance.OnPlayerHit();

        if (damage >= 10)
        {
            if (knockback >= 0 && knockback < 5)
            {
                _ienemy.SKnockBack(dir, knockback);
            }
            if (knockback >= 5)
            {
                Debug.Log("Bkock");
                _ienemy.BKnockBack(dir, knockback);
            }
        }

        Debug.Log("<color=red>" + gameObject.name + " (敵) が" + damage + "ダメージ受けました。残りHP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void TakeElectDamage(int damage,int knockback,int dir, float electtime, string audioname)
    {
        if (currentHP <= 0) return;

        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0); // 0未満にならないようにクランプ
        GManager.Instance.OnPlayerHit();

        _ienemy.ElectStun(dir, knockback, electtime);
  

        var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == "DamageEffect");//ラムダ形式AIで知った
        if (attackData != null && attackData.HitEffect != null)
        {
            var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.2f);
        }

        Debug.Log("<color=red>" + gameObject.name + " (敵) が" + damage + "ダメージ受けました。残りHP: " + currentHP);

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
        Debug.Log("<color=red>" + gameObject.name + " (敵) は倒されました！");

        // 独自の敵の死亡処理を記述
        // 例1: スコアを加算する処理
        // GameManager.Instance.AddScore(100); 

        // 例2: 爆発エフェクトを再生する処理
        // Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // 例3: オブジェクトをシーンから破壊
        Destroy(gameObject);
    }
}
