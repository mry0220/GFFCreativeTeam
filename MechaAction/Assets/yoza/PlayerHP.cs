using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    // [SerializeField] でInspectorから設定可能にする
    [Header("Health Values")]
    [SerializeField] private float maxHealth = 100f;

    // 現在のHPはprivate変数で管理
    private float currentHealth;

    // 現在のHPを他のスクリプト（UIなど）から取得するための公開プロパティ
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    void Start()
    {
        // ゲーム開始時にHPを最大値に設定
        currentHealth = maxHealth;
        Debug.Log(gameObject.name + "のHPが初期化されました: " + currentHealth);
    }

   
    //ダメージを受け、HPを減少させる。
    public void TakeDamage(float damageAmount)
    {
        if (currentHealth <= 0)
        {
            // 既に死亡している場合は処理をスキップ
            return;
        }

        currentHealth -= damageAmount;

        // HPが0未満にならないように制限
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log(gameObject.name + "が" + damageAmount + "ダメージ受けました。残りHP: " + currentHealth);

        // HPが0以下になったかチェック
        if (currentHealth <= 0)
        {
            Die();
        }
    }

   
    // HPを回復させる。
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;

        // HPが最大値を超えないように制限
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log(gameObject.name + "のHPが" + healAmount + "回復しました。残りHP: " + currentHealth);
    }

    
    // HPが0になったときの死亡処理。（プレイヤー専用）
    private void Die()
    {
        Debug.Log(gameObject.name + "は倒されました。ゲームオーバー！");
        // ここにゲームオーバー画面の表示、リスタート処理、プレイヤー入力の無効化などの処理を追加
        gameObject.SetActive(false); // 例としてプレイヤーオブジェクトを非アクティブ化
    }
}