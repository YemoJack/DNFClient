using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;

/// <summary>
/// 技能状态
/// </summary>
public enum SkillState
{
    None,
    Befor,//释放前摇
    After,//释放后摇
    End//释放结束
}




public partial class Skill 
{
    /// <summary>
    /// 技能id
    /// </summary>
    public int mSkillid;
    /// <summary>
    /// 技能创建者
    /// </summary>
    private LogicActor mSkillCreater;

    /// <summary>
    /// 技能数据
    /// </summary>
    private SkillDataConfig mSkillData;

    /// <summary>
    /// 释放技能后摇
    /// </summary>
    public Action<Skill> OnReleaseAfter;

    /// <summary>
    /// 释放技能结束回调
    /// </summary>
    public Action<Skill, bool> OnReleaseSkillEnd;

    /// <summary>
    /// 技能状态
    /// </summary>
    public SkillState skillState = SkillState.None;

    /// <summary>
    /// 当前逻辑帧
    /// </summary>
    private int mCurLogicFrame = 0;

    /// <summary>
    /// 当前累计运行时间
    /// </summary>
    private int mCurLogicFrameAccTime = 0;

    /// <summary>
    /// 技能构造函数
    /// </summary>
    /// <param name="skillid"></param>
    /// <param name="skillCreater"></param>
    public Skill(int skillid,LogicActor skillCreater)
    {
        mSkillid = skillid;
        mSkillCreater = skillCreater;
        mSkillData = ZMAssetsFrame.LoadScriptableObject<SkillDataConfig>(AssetPathConfig.SKILL_DATA_PATH + skillid + ".asset");
    }


    /// <summary>
    /// 释放技能
    /// </summary>
    /// <param name="OnReleaseAfter">释放技能后摇</param>
    /// <param name="OnReleaseSkillEnd">释放技能结束</param>
    public void ReleaseSkill(Action<Skill> OnReleaseAfter,Action<Skill, bool> OnReleaseSkillEnd)
    {
        this.OnReleaseAfter = OnReleaseAfter;
        this.OnReleaseSkillEnd = OnReleaseSkillEnd;
        
        SkillStart();
        skillState = SkillState.Befor;
        PlayAnim();
    }

    /// <summary>
    /// 播放释放动画
    /// </summary>
    public void PlayAnim()
    {
        // 播放角色动画
        mSkillCreater.PlayAnim(mSkillData.character.SkillAnim);
    }

    /// <summary>
    /// 释放技能前摇
    /// </summary>
    public void SkillStart()
    {
        //开始释放技能时，进行初始化
        mCurLogicFrame = 0;
        mCurLogicFrameAccTime = 0;
    }

    /// <summary>
    /// 释放技能后摇
    /// </summary>
    public void SkillAfter()
    {
        skillState = SkillState.After;
    }

    /// <summary>
    /// 释放技能结束
    /// </summary>
    public void SkillEnd()
    {
        skillState = SkillState.End;
        OnReleaseSkillEnd?.Invoke(this, false); //TODO:是否为组合技能，以后再处理
    }


    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdate()
    {

        if(skillState == SkillState.None)
        {
            return;
        }    

        //  计算累计时间
        mCurLogicFrameAccTime += mCurLogicFrame * LogicFrameConfig.LogicFrameIntervalMS;

        if(skillState == SkillState.Befor&& mCurLogicFrameAccTime >= mSkillData.skillCfg.skillShakeAfterMs)
        {
            SkillAfter();
        }

        //更新不同配置的逻辑帧，处理不同配置的逻辑

        //更新特效逻辑帧
        OnLogicFrameUpdateEffect();
        //更新伤害逻辑帧
        OnLogicFrameUpdateDamage();
        //更新行动逻辑帧

        //更新音效逻辑帧

        //更新子弹逻辑帧


        //判断技能是否释放结束
        if(mCurLogicFrame == mSkillData.character.logicFrame)
        {
            SkillEnd();
        }

        // 当前逻辑帧自增
        mCurLogicFrame++;
    }


}
