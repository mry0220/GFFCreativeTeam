using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP_T : MonoBehaviour
{
    // [SerializeField] でInspectorから設定可能にする
    [SerializeField] private float maxHP = 100f;

    // 現在のHPはprivate変数で管理
    private float currentHP;

    // 現在のHPを他のスクリプト（UIなど）から取得するための公開プロパティ
    public float CurrentHP => currentHP;
    public float MaxHP => maxHP;

    void Start()
    {
        // ゲーム開始時にHPを最大値に設定
        currentHP = maxHP;
        Debug.Log(gameObject.name + "のHPが初期化されました: " + currentHP);
    }


    //ダメージを受け、HPを減少させる。
    public void TakeDamage(float damageAmount)
    {
        if (currentHP <= 0)
        {
            // 既に死亡している場合は処理をスキップ
            return;
        }


        // --- ここからダメージ処理 ---
        currentHP -= damageAmount;
        // HPが0未満にならないように制限
        currentHP = Mathf.Max(currentHP, 0);

        Debug.Log(gameObject.name + "が" + damageAmount + "ダメージ受けました。残りHP: " + currentHP);

        // HPが0以下になったかチェック
        if (currentHP <= 0)
        {
            Die();
        }
    }


    // HPを回復させる。
    public void Heal(float healAmount)
    {
        currentHP += healAmount;

        // HPが最大値を超えないように制限
        currentHP = Mathf.Min(currentHP, maxHP);

        Debug.Log(gameObject.name + "のHPが" + healAmount + "回復しました。残りHP: " + currentHP);
    }


    // HPが0になったときの死亡処理。（プレイヤー専用）
    private void Die()
    {
        Debug.Log(gameObject.name + "は倒されました。ゲームオーバー！");
        // ここにゲームオーバー画面の表示、リスタート処理、プレイヤー入力の無効化などの処理を追加
        // プレイヤーオブジェクトを非アクティブ化
        gameObject.SetActive(false);
    }
}
