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
        foreach(var skill in mSkillArr)
        {
            if(skill.skillid == skillId)
            {

                if(skill.skillState != SkillState.None && skill.skillState != SkillState.End)
                {
                    Debug.Log($"技能{skill.skillid} 正在释放中 不能再次释放");
                    return null;
                }

                //释放技能
                skill.ReleaseSkill(OnReleaseAfter, (skill, combinationSkill) =>
                {
                    //技能释放结束
                    OnReleaseSkillEnd?.Invoke(skill);
                    //是否是连招
                    if (combinationSkill)
                    {

                    }
                });
                
                return skill;
            }
        }
        Debug.LogError($"没有找到id为{skillId}的技能");
        return null;
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
