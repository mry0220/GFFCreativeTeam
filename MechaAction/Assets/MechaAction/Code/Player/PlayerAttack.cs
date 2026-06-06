using System.Collections.Generic;
using UnityEngine;

public enum PlayerAttackType
{
    NormalAttack,

}

public class PlayerAttack : MonoBehaviour
{
    //component-----------------
    //private Entity m_entity;
    //--------------------------

    //attackData----------------
    [SerializeField] private AttackDataSO m_normalAttackData;
    [SerializeField] private HitCollider m_normalCollider;

    private void Awake()
    {
        //m_entity = GetComponent<Entity>();
    }

    public void OnAttack(PlayerAttackType type)
    {
        switch(type)
        {
            case PlayerAttackType.NormalAttack:

                break;
        }
    }
}
