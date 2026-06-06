using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private DamageEventSO m_damageEventSO;
    [SerializeField] private AudioEventSO m_bgmEventSO;
    [SerializeField] private AudioEventSO m_seEventSO;

    [SerializeField] private AudioSource m_BGM;
    [SerializeField] private AudioSource m_SE;

    private void OnEnable()
    {
        m_damageEventSO.Register(AudioDamagePlay);
        m_bgmEventSO.Register(AudioBGMPlay);
        m_seEventSO.Register(AudioSEPlay);
    }

    private void OnDisable()
    {
        m_damageEventSO.Unregister(AudioDamagePlay);
        m_bgmEventSO.Unregister(AudioBGMPlay);
        m_seEventSO.Unregister(AudioSEPlay);
    }

    private void Awake()
    {
        //if(Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }

    public void AudioDamagePlay(EffectEvent d_event)
    {
        //if(d_event.audioData == null) return;

        //m_SE.PlayOneShot(d_event.audioData.Clip);
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
