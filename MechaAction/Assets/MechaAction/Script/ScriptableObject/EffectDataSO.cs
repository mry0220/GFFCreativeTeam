using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "SctiptableObjects/EffectData")]
public class EffectDataSO : ScriptableObject
{
    [SerializeField] private GameObject m_prefab;
    [SerializeField] private float m_duration;

    public GameObject EffectPrefab { get => m_prefab; }
    public float Duration { get => m_duration; }
}
