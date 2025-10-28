using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamage
{
    // 現在のHPと最大HPを取得するためのプロパティ
    float MaxHP { get; }
    float CurrentHP { get; }

    // HPを操作するためのメソッド
    void PlayerDamage(float amount);
    void Heal(float amount);
    void Die();

}
