using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playertest : MonoBehaviour
{
    // PlayerHPクラスを直接参照
    public PlayerHP targetHealthManager;

  
    [SerializeField]public float testDamage = 10f;
    [SerializeField]public float testHeal = 5f;

    void Update()
    {
        if (targetHealthManager == null) return;

        // Dキーでダメージをテスト
        if (Input.GetKeyDown(KeyCode.D))
        {
            targetHealthManager.TakeDamage(testDamage);
        }

        // Hキーで回復をテスト
        if (Input.GetKeyDown(KeyCode.H))
        {
            targetHealthManager.Heal(testHeal);
        }
    }
}