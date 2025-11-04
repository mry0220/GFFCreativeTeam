using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerDamage 
{
    void TakeDamage(int amount, int knockback, int dir, string name);

    void TakeElectDamage(int amount,float electtime, string name);

    void TakeBanDamage(float bantime, string name);

    void Heal(int amount);
    void Die();
}
