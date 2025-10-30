using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void TakeDamage(int amount,int knockback,int dir);

    void Heal(float amount);
    void Die();
}
