using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Attack,//基礎
    Hp,
    Gun,//基礎
    Speed,
    UNB,
    Skill,
    Knockback,
    //武器ごと
    Elect,
    Ground,
    ShotGun,
    Rifle
}

public class SkillState//状態
{
    public SkillDataSO data;
    public bool isUnlocked;

    public SkillState(SkillDataSO data)
    {
        this.data = data;
        isUnlocked = false;
    }
}

public class SkillManager : MonoBehaviour
{
    [SerializeField] private SkillDataSO[] m_allSkill;

    private Dictionary<SkillDataSO, SkillState> m_skillStates =
        new Dictionary<SkillDataSO, SkillState>();

    [Header("Event")]
    [SerializeField] private SkillEventSO m_skillUnlockEvent;//取得したさい
    [SerializeField] private SkillEventSO m_skillEvent;//関数を入れる

    [Header("AudioEvent")]
    [SerializeField] private AudioEventSO m_audioEvent;
    [SerializeField] private AudioDataSO m_audioGetData;
    [SerializeField] private AudioDataSO m_audioNotData;

    [Header("SaveManager")]
    [SerializeField] private SaveManager m_saveManager;

    private int m_score;

    private void OnEnable()
    {
        m_skillEvent.Register(TryUnlockSkill);
    }

    private void OnDisable()
    {
        m_skillEvent.Unregister(TryUnlockSkill);
    }

    private void Awake()
    {
        m_skillStates = new Dictionary<SkillDataSO, SkillState>();

        foreach(var skill in m_allSkill)
        {
            m_skillStates.Add(skill, new SkillState(skill));
        }
    }

    public void TryUnlockSkill(SkillDataSO data)
    {
        if (!m_skillStates.ContainsKey(data)) return;

        var state = m_skillStates[data];

        if (!CanUnlock(data)) return;

        state.isUnlocked = true;

        m_skillUnlockEvent.Raise(data);//button animation
        SkillAudio(m_audioGetData);//取得
    }

    private bool CanUnlock(SkillDataSO data)
    {
        var state = m_skillStates[data];

        if (state.isUnlocked) return false;//取得済み

        foreach(var skill in data.NeedSkill)//前提条件
        {
            if (!m_skillStates[skill].isUnlocked) return false;
        }

        if (m_score < data.Cost)
        {
            SkillAudio(m_audioNotData);
            return false;//コスト不足
        }
        m_score -= data.Cost;

        return true;
    }

    public SkillState GetState(SkillDataSO data)//loadした後呼ばれる必要
    {
        return m_skillStates[data];
    }

    public void SkillAudio(AudioDataSO data)
    {
        m_audioEvent.Raise(data);
    }

    public void SkillSave()//GameManagerで呼んでもいいEventでもいい
    {
        var saveData = GetSaveData();
        m_saveManager.SkillSave(saveData);
    }

    public void SkillLoad()
    {
        var data = m_saveManager.SkillLoad();
        SkillLoadDataList(data);
    }

    private SkillSaveDataList GetSaveData()
    {
        SkillSaveDataList saveList = new SkillSaveDataList();

        foreach(var skill in m_skillStates)
        {
            //var skillData = skill.Key;//SkillDataSO
            var state = skill.Value;//SkillState

            SkillSaveData data = new SkillSaveData
            {
                ID = state.data.ID,
                isUnlocked = state.isUnlocked
            };

            saveList.m_skillDataList.Add(data);
        }

        return saveList;
    }

    private void SkillLoadDataList(SkillSaveDataList data)
    {
        foreach(var saveData  in data.m_skillDataList)
        {
            var skillData = FindSkillByID(saveData.ID);

            if(skillData != null)
            {
                m_skillStates[skillData].isUnlocked = saveData.isUnlocked;
            }
        }
    }

    private SkillDataSO FindSkillByID(int ID)//IDにあうSkillDataSOを探す
    {
        foreach(var skill in m_skillStates.Values)
        {
            if (skill.data.ID == ID) return skill.data;
        }

        return null;
    }
}
