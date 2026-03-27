using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "SctiptableObjects/AudioData")]
public class AudioDataSO : ScriptableObject
{
    [SerializeField] private AudioClip m_clip;
    [SerializeField] private float m_volum;

    public AudioClip Clip { get => m_clip; }
    public float volum { get => m_volum; }
}
