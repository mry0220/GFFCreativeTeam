using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerDamage 
{
    void TakeDamage(int damage, int knockback, int dir, string effectname, string audioname);

    void TakeElectDamage(int damage,int knockback,int dir, float electtime, string effectname, string audioname);

    void TakeBanDamage(float bantime, string effectname, string audioname);

    void Heal(int damage, string effectname, string audioname);
    void Die();
}
