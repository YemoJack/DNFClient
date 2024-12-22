using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 技能系统
/// </summary>
public class SkillSystem 
{
    /// <summary>
    /// 技能创建者
    /// </summary>
    private LogicActor mSkillCreater;

    private List<Skill> mSkillArr = new List<Skill>();

    /// <summary>
    /// 当前释放中的技能
    /// </summary>
    private Skill mCurSkill;
    /// <summary>
    /// 技能组合技id列表
    /// </summary>
    private List<int> mCombinationSkillIdList = new List<int>();


    public SkillSystem(LogicActor skillCreater)
    {
        mSkillCreater = skillCreater;
    }

    /// <summary>
    /// 初始化技能
    /// </summary>
    /// <param name="skillId">技能id数组</param>
    public void InitSkills(int[] skillIdArr)
    {
        foreach(var skillid in skillIdArr)
        {
            Skill skill = new Skill(skillid, mSkillCreater);
            mSkillArr.Add(skill);
            if(skill.SkillConfig.ComobinationSkillid!= 0)
            {
                //使用递归方式调用技能组合技所有技能的初始化
                InitSkills(new int[] { skill.SkillConfig.ComobinationSkillid });
            }
            //初始化所有蓄力技能
            if(skill.SkillConfig.stockPileStageData.Count > 0)
            {
                foreach(var item in skill.SkillConfig.stockPileStageData)
                {
                    InitSkills(new int[] {item.skillid});
                }
            }
        }

        Debug.Log($"技能初始化完成 技能个数：{skillIdArr.Length}");
    }

    /// <summary>
    /// 释放技能
    /// </summary>
    /// <param name="skillId"></param>
    /// <param name="OnReleaseAfter"></param>
    /// <param name="OnReleaseSkillEnd"></param>
    public Skill ReleaseSkill(int skillId,Action<Skill> OnReleaseAfter,Action<Skill> OnReleaseSkillEnd)
    {
        //如果当前技能不为空，并且当前技能处在开始释放或释放前摇的时候 不允许释放其他技能
        if(mCurSkill != null && (mCurSkill.skillState != SkillState.End && mCurSkill.skillState != SkillState.After))
        {
            return null;
        }
        //组合技释放中，非组合技能无法释放
        if(mCombinationSkillIdList.Count > 0 && !mCombinationSkillIdList.Contains(skillId))
        {
            return null;
        }


        foreach(var skill in mSkillArr)
        {
            if(skill.skillid == skillId)
            {

                if(skill.skillState != SkillState.None && skill.skillState != SkillState.End)
                {
                    Debug.Log($"技能{skill.skillid} 正在释放中 不能再次释放");
                    return null;
                }

                //判断当前技能是否有组合技
                if(skill.SkillConfig.ComobinationSkillid != 0)
                {
                    CacleteCombinationSkillIdList(skillId);
                }


                //释放技能
                skill.ReleaseSkill(OnReleaseAfter, (skill, combinationSkill) =>
                {
                    //技能释放结束
                    OnReleaseSkillEnd?.Invoke(skill);
                    //是否是连招
                    if (!combinationSkill)
                    {
                        //技能释放完成 比如组合技的最后一个技能释放完成
                        mCurSkill = null;
                        if(skill.SkillConfig.ComobinationSkillid == 0 && mCombinationSkillIdList.Count > 0)
                        {
                            mCombinationSkillIdList.Clear();
                        }
                    }
                });
                mCurSkill = skill;
                return skill;
            }
        }
        Debug.LogError($"没有找到id为{skillId}的技能");
        return null;
    }


    /// <summary>
    /// 主动触发蓄力技能
    /// </summary>
    public void TriggerStockPileSkill(int skillId)
    {
        //如果当前技能不为空，并且当前技能处在开始释放或释放前摇的时候 不允许释放其他技能
        if (mCurSkill != null && mCurSkill.skillid != skillId)
        {
            return;
        }
        //判断当前是否有组合技正在释放 如果有就不允许释放其他技能
        //组合技释放中，非组合技能无法释放
        if (mCombinationSkillIdList.Count > 0 && !mCombinationSkillIdList.Contains(skillId))
        {
            return;
        }
        Skill skill = GetSkill(skillId);
        if(skill != null)
        {
            skill.TriggerStockPileSkill();
        }
    }

    /// <summary>
    /// 计算组合技能ID列表
    /// </summary>
    /// <param name="skillId"></param>
    public void CacleteCombinationSkillIdList(int skillId)
    {
        if(skillId != 0)
        {
            int combinationSkillId = skillId;
            while(combinationSkillId != 0)
            {
                mCombinationSkillIdList.Add(combinationSkillId);
                combinationSkillId = GetSkill(combinationSkillId).SkillConfig.ComobinationSkillid;
            }
        }
        else
        {
            Debug.LogError("无效技能id " + skillId);
        }
    }





    public Skill GetSkill(int skillId)
    {
        foreach(var skill in mSkillArr)
        {
            if(skill.skillid == skillId)
            { return skill; }
        }
        return null;
    }



    public void OnLogicFrameUpdate()
    {
        foreach(var skill in mSkillArr)
        {
            skill.OnLogicFrameUpdate();
        }
    }



}
