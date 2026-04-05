using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Reference Event")]
    [SerializeField] private BoolEvent m_eventGameOverUI;
    [SerializeField] private BoolEvent m_eventEnhanceUI;
    [SerializeField] private BoolEvent m_eventMenuUI;
    [SerializeField] private BoolEvent m_eventOptionUI;
    [SerializeField] private BoolEvent m_eventTutorialUI;

    [Header("Reference UI")]
    [SerializeField] private GameObject m_gameOverUI;
    [SerializeField] private GameObject m_enhanceUI;
    [SerializeField] private GameObject m_menuUI;
    [SerializeField] private GameObject m_optionUI;
    [SerializeField] private GameObject m_tutorialUI;

    private void OnEnable()
    {
        m_eventGameOverUI.Register(GameOverUI);
        m_eventEnhanceUI.Register(EnhanceUI);
        m_eventMenuUI.Register(MenuUI);
        m_eventOptionUI.Register(OptionUI);
        m_eventTutorialUI.Register(TutorialUI);
    }

    private void OnDisable()
    {
        m_eventGameOverUI.Unregister(GameOverUI);
        m_eventEnhanceUI.Unregister(EnhanceUI);
        m_eventMenuUI.Unregister(MenuUI);
        m_eventOptionUI.Unregister(OptionUI);
        m_eventTutorialUI.Unregister(TutorialUI);
    }

    private void Start()
    {
        GameOverUI(false);
        EnhanceUI(false);
        MenuUI(false);
        OptionUI(false);
        TutorialUI(false);
    }

    public void GameOverUI(bool isbool)
    {
        m_gameOverUI.SetActive(isbool);
    }

    public void EnhanceUI(bool isbool)
    {
        m_enhanceUI.SetActive(isbool);
    }

    public void MenuUI(bool isbool)
    {
        m_menuUI.SetActive(isbool);
    }

    public void OptionUI(bool isbool)
    {
        m_optionUI.SetActive(isbool);
    }

    public void TutorialUI(bool isbool)
    {
        m_tutorialUI.SetActive(isbool);
    }
}
