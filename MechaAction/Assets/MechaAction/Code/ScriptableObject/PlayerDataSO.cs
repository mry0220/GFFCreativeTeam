using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data",menuName = "SctiptableObjects/Datas/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    [SerializeField] private int m_maxHp;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_jumpPower;

    public int MaxHP { get => m_maxHp; }
    public float Speed { get => m_speed; }
    public float JumpPower { get => m_jumpPower; }
}
