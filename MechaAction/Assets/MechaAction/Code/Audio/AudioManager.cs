using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource m_BGM;
    [SerializeField] private AudioSource m_SE;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AudioBGMPlay(AudioDataSO data)
    {
        if(data == null) return;

        if(!m_BGM.isPlaying)
        {
            m_BGM.clip = data.Clip;
            m_BGM.Play();
        }
    }

    public void AudioSEPlay(AudioDataSO data)
    {
        if(data == null) return;

        m_SE.PlayOneShot(data.Clip);
    }
}
