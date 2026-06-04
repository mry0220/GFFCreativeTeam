using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy 
{
    void KnockBack(Vector3 attackDir, int knockback);

    void ElectStun(float duration);
}
