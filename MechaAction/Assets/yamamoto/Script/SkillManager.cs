using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillType
{
    ATTACK,
    COMBO,
    DRAGON,
}

public class SkillManager : MonoBehaviour
{
    [SerializeField] Text skillPointText;
    [SerializeField] Text skillInfoText;
    [SerializeField] GameObject skillBlockPanel;
    int skillPoint = 10;

    [SerializeField] List<SkillType> skillList = new List<SkillType>();//購入済みを把握するlist
    SkillBlock[] skillBlocks;

    public static SkillManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        skillBlocks = skillBlockPanel.GetComponentsInChildren<SkillBlock>();
    }

    private void Start()
    {
        UpdateSkillPointText();
        UpdateSkillInfoText();
    }

    void UpdateSkillPointText()
    {
        skillPointText.text = string.Format("消費可能ポイント：{0}", skillPoint);
    }
    public void UpdateSkillInfoText(string text ="")
    {
        skillInfoText.text = text;
    }

    public bool HasSkill(SkillType skillType)//listのなかにあるかどうか
    {
        return skillList.Contains(skillType);
    }

    public bool CanLearnSkill(int cost, SkillType skillType)//コストが足りているか
    {
        if (skillPoint < cost)
        {
            return false;
        }

        //if (skillType == SkillType.COMBO)
        //{
        //    return HasSkill(SkillType.ATTACK) && HasSkill(SkillType.DEFENSE);
        //}
        if (skillType == SkillType.COMBO) //コンボを取得したいときAttackは取得しているか
        {
            return HasSkill(SkillType.ATTACK);
        }
        if (skillType == SkillType.DRAGON)
        {
            return HasSkill(SkillType.COMBO);
        }
        return true;
    }

    public void LearnSkill(int cost, SkillType skillType)
    {
        skillList.Add(skillType);
        CheckActiveBlocks();
        skillPoint -= cost;
        UpdateSkillPointText();
    }

    void CheckActiveBlocks()
    {
        foreach (SkillBlock skillBlock in skillBlocks)
        {
            skillBlock.CheckActiveBlock();
        }
    }
}
