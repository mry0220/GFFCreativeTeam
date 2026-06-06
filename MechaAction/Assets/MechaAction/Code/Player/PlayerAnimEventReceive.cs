using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEventReceive : MonoBehaviour
{
    private PlayerT m_player;
    private Player_AttackT m_attack;

    private void Awake()
    {
        m_player = GetComponentInParent<PlayerT>();
        m_attack = GetComponentInParent<Player_AttackT>();
    }

    public void OnReturnState()
    {
        m_player._ReturnNormal();
    }

    public void OnGroundAttack()
    {
        m_attack.GroundAttack();
    }
}
