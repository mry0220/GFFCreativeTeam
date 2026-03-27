using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data",menuName = "SctiptableObjects/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    [SerializeField] private int m_maxHp;
    [SerializeField] private int m_attack;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_jumpPower;

    public int MaxHP { get => m_maxHp; }
    public int Attack { get => m_attack; }
    public float Speed { get => m_speed; }
    public float JumpPower { get => m_jumpPower; }
}
