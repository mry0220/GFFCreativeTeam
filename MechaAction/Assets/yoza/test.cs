using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Inspectorからアタッチされたオブジェクトをアサイン
    
    public GameObject targetObject;
    private keiyaku targetHealth; // keiyakuから参照を持つ

    [Header("Test\nTキーでダメージ\nHキーでヒール")]
    public float testDamage = 15f;
    public float testHeal = 10f;

    void Start()
    {
        // ターゲットオブジェクトからIHealthコンポーネントを取得
        if (targetObject != null)
        {
            targetHealth = targetObject.GetComponent<keiyaku>();
        }

        if (targetHealth == null)
        {
            Debug.LogError("テスト対象オブジェクトにIHealthを実装したコンポーネントが見つかりません！");
            enabled = false; // スクリプトを無効化
        }
    }

    void Update()
    {
        if (targetHealth == null) return;

        // Dキー でダメージをテスト
         else if (Input.GetKeyDown(KeyCode.D))
        {
            targetHealth.TakeDamage(testDamage);
        }

        // Hキーで回復をテスト
        else if (Input.GetKeyDown(KeyCode.H))
        {
            targetHealth.Heal(testHeal);
        }
    }
}
