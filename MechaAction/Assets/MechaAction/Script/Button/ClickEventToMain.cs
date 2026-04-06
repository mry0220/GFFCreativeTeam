using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickEventToMain : MonoBehaviour
{
    [SerializeField] private AudioEventSO m_eventAudioSE;

    [Header("TitleScene")]
    [SerializeField] private StringRunTime m_titleString;
    [SerializeField] private AudioDataSO m_audioDataToTitle;

    public void OnClickToTitleScene()
    {
        string name = m_titleString.Value;
        m_eventAudioSE.Raise(m_audioDataToTitle);
        //Event
        SceneManager.LoadScene(name);
    }

    [Header("ReloadScene")]
    [SerializeField] private AudioDataSO m_audioDataToReload;
    public void OnClickToReloadScene()
    {
        m_eventAudioSE.Raise(m_audioDataToReload);
        //Event
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [Header("Option")]
    [SerializeField] private AudioDataSO m_audioDataToOption;
    [SerializeField] private BoolEvent m_eventOptionUI;
    public void OnClickToOption()
    {
        m_eventAudioSE.Raise(m_audioDataToOption);
        //Event
        m_eventOptionUI.Raise(true);
    }

    [Header("CommandCheck")]
    [SerializeField] private AudioDataSO m_audioDataToCommandCheck;
    [SerializeField] private BoolRunTimeSO m_IsCommandCheck;

    public void OnClickToCommandCheckInOption(bool isbool)
    {
        m_eventAudioSE.Raise(m_audioDataToCommandCheck);
        //RunTime
        m_IsCommandCheck.SetValue(isbool);
    }

    //command check sheet
}
