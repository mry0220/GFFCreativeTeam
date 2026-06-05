using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEventReceive : MonoBehaviour
{
    private PlayerT m_player;
    private Player_Attack m_attack;

    private void Awake()
    {
        m_player = GetComponentInParent<PlayerT>();
        m_attack = GetComponentInParent<Player_Attack>();
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
