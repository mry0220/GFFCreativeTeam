using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillBlock : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pointText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] SkillType skillType;
    [SerializeField] int cost;
    [SerializeField] string costtext;
    [SerializeField] new string name;
    [SerializeField] string info;
    [SerializeField] GameObject hidePanel;

    [SerializeField] private SkillManager m_skillManager;

    [SerializeField] private SkillDataSO m_skillData;

    [Header("Event")]
    [SerializeField] private SkillEventSO m_skillEvent;//ボタンを押した際
    [SerializeField] private SkillEventSO m_skillUnlockEvent;//スキル獲得時の

    private void OnEnable()
    {
        m_skillUnlockEvent.Register(ChengeColorUI);
        m_skillUnlockEvent.Register(PlayAnim);
    }

    private void OnDisable()
    {
        m_skillUnlockEvent.Unregister(ChengeColorUI);
        m_skillUnlockEvent.Unregister(PlayAnim);
    }

    private void Start()
    {
        var state = m_skillManager.GetState(m_skillData);

        if(state.isUnlocked)
        {

        }
        else
        {

        }
    }

    public void OnSkillEvent()
    {
        m_skillEvent.Raise(m_skillData);
    }

    private void ChengeColorUI(SkillDataSO skill)
    {
        if(skill == m_skillData)
        {
            
        }
    }

    private void PlayAnim(SkillDataSO skill)
    {
        if(skill == m_skillData)
        {

        }
    }
}
