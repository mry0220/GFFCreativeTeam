using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickEventToTitle : MonoBehaviour
{
    [SerializeField] private AudioEventSO m_eventAudioSE;

    [Header("MainScene")]
    [SerializeField] private StringRunTime m_startString;
    [SerializeField] private AudioDataSO m_audioDataToMain;

    public void OnClickToMainScene()
    {
        string name = m_startString.Value;
        m_eventAudioSE.Raise(m_audioDataToMain);
        //Event
        SceneManager.LoadScene(name);
    }

    [Header("ResetData")]
    [SerializeField] private AudioDataSO m_audioDataToReset;

    public void OnClickToResetData()
    {
        m_eventAudioSE.Raise(m_audioDataToReset);
        //SaveManager
    }

    [Header("QuitGame")]
    [SerializeField] private AudioDataSO m_audioDataToQuit;

    public void OnClickToQuitGame()
    {
        m_eventAudioSE.Raise(m_audioDataToQuit);

        Application.Quit();

        // エディタで止めるため（便利）
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
