using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    //1回の攻撃で同じ敵に何度も当たり判定が入らないようにする
    private HashSet<GameObject> hitTargets = new HashSet<GameObject>();
    public int damage = 10;//プレイヤースクリプトで技によって変更

    void OnEnable()
    {
        // 攻撃開始時にリストをリセット
        hitTargets.Clear();
    }

    //問題　連続した攻撃の際、アニメーションのシグナルか何かでClear();を呼ぶ必要あり


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !hitTargets.Contains(other.gameObject))
        {
            //other.GetComponent<IDamage>().Damage(damage);//敵のインターフェース<IDamage>取得
            hitTargets.Add(other.gameObject);
        }
    }
}
/*
 プレイヤーのスクリプト
[SerializeField] private SwordHitbox swordHitbox;

public void DoAttack(string attackType)
{
    int damageValue = 0;

    switch (attackType
    {
        case "LightAttack":
            damageValue = 10;
            break;
        case "HeavyAttack":
            damageValue = 25;
            break;
        case "SpecialAttack":
            damageValue = 50;
            break;
    }

    // ダメージ値を渡す
    swordHitbox.damage = damageValue;

    // ヒットボックスを有効化（アニメーションイベントなどで管理すると自然）
    swordHitbox.gameObject.SetActive(true);
*/