using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDamage : MonoBehaviour
{
    [SerializeField] public float damageAmount = 20f;

    // ColliderのIs Triggerにチェックが入っている場合、他のColliderと接触すると呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        // 衝突した相手のオブジェクトが「Player」タグを持っているか確認
        if (other.gameObject.CompareTag("Player"))
        {
            // 衝突した相手からPlayerHealthSimpleコンポーネントを直接取得
            PlayerHP playerHealth = other.GetComponent<PlayerHP>();

            // PlayerHealthSimpleコンポーネントがアタッチされていれば
            if (playerHealth != null)
            {
                // ダメージを与える
                playerHealth.TakeDamage(damageAmount);

                // 弾丸などの場合は、ダメージを与えた後、自身を破壊する
                //Destroy(gameObject);
            }
        }
    }
}