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



/// <summary>
/// 技能
/// </summary>
public partial class Skill 
{
    /// <summary>
    /// 技能id
    /// </summary>
    public int skillid;
    /// <summary>
    /// 技能创建者
    /// </summary>
    private LogicActor mSkillCreater;

    /// <summary>
    /// 技能数据
    /// </summary>
    private SkillDataConfig mSkillData;

    public SkillConfig SkillConfig { get { return mSkillData.skillCfg; } }

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
    /// 是否自动匹配蓄力阶段
    /// </summary>
    private bool mAutoMacthStockStage;



    /// <summary>
    /// 技能构造函数
    /// </summary>
    /// <param name="skillid"></param>
    /// <param name="skillCreater"></param>
    public Skill(int skillid,LogicActor skillCreater)
    {
        this.skillid = skillid;
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
        mAutoMacthStockStage = false;
    }

    /// <summary>
    /// 释放技能后摇
    /// </summary>
    public void SkillAfter()
    {
        skillState = SkillState.After;
        OnReleaseAfter?.Invoke(this);
    }

    /// <summary>
    /// 释放技能结束
    /// </summary>
    public void SkillEnd()
    {
        skillState = SkillState.End;
        OnReleaseSkillEnd?.Invoke(this, mSkillData.skillCfg.ComobinationSkillid != 0); //TODO:是否为组合技能，以后再处理
        ReleaseAllEffect();
        if(mSkillData.skillCfg.ComobinationSkillid != 0)
        {
            mSkillCreater.ReleaseSkill(mSkillData.skillCfg.ComobinationSkillid);
        }
    }


    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdate()
    {

        if(skillState == SkillState.None || skillState == SkillState.End)
        {
            return;
        }    

        //  计算累计时间
        mCurLogicFrameAccTime +=  LogicFrameConfig.LogicFrameIntervalMS;

        if(skillState == SkillState.Befor&& mCurLogicFrameAccTime >= mSkillData.skillCfg.skillShakeAfterMs && mSkillData.skillCfg.skillType != SkillType.StockPlie)
        {
            SkillAfter();
        }

        //更新不同配置的逻辑帧，处理不同配置的逻辑

        //更新特效逻辑帧
        OnLogicFrameUpdateEffect();
        //更新伤害逻辑帧
        OnLogicFrameUpdateDamage();
        //更新行动逻辑帧
        OnLogicFrameUpdateAction();
        //更新音效逻辑帧
        OnLogicFrameUpdateAudio();
        //更新子弹逻辑帧


        //因为蓄力技能需要通过蓄力时间进行触发，所以和技能的结束帧无关
        if(mSkillData.skillCfg.skillType == SkillType.StockPlie)
        {
            int stockDataCount = mSkillData.skillCfg.stockPileStageData.Count;
            if(stockDataCount > 0)
            {
                //处理手指按下立马抬起的情况
                if (mAutoMacthStockStage)
                {
                    StockPileStageData stockData = mSkillData.skillCfg.stockPileStageData[0];
                    //自动匹配第一阶段蓄力技能
                    if (mCurLogicFrameAccTime >= stockData.startTimeMs)
                    {
                        StockPileFinish(stockData);

                    }
                }
                else
                {
                    //处理超时蓄力的逻辑
                    StockPileStageData stockData = mSkillData.skillCfg.stockPileStageData[stockDataCount - 1];
                    //计算蓄力使劲按是否达到最大值，如果达到最大值就自动触发最大蓄力阶段技能（比如用户按着蓄力技能按钮不松）
                    if (mCurLogicFrameAccTime >= stockData.endTimeMs)
                    {
                        StockPileFinish(stockData);
                    }


                }


            }
        }
        else
        {
            //判断技能是否释放结束
            if (mCurLogicFrame == mSkillData.character.logicFrame)
            {
                SkillEnd();
            }
        }

        

        // 当前逻辑帧自增
        mCurLogicFrame++;
    }

    /// <summary>
    /// 主动触发蓄力技能
    /// </summary>
    public void TriggerStockPileSkill()
    {
        //1. 蓄力时间符合蓄力阶段配置中的某一个技能
        foreach(var item in mSkillData.skillCfg.stockPileStageData)
        {
            if(mCurLogicFrameAccTime >= item.startTimeMs && mCurLogicFrameAccTime <= item.endTimeMs)
            {
                StockPileFinish(item);
                return;
            }
        }

        //2. 蓄力时间过短，不符合任意蓄力阶段的技能，自动匹配第一阶段技能
        mAutoMacthStockStage = true;
    }



    /// <summary>
    /// 蓄力技能完成
    /// </summary>
    /// <param name="stockData"></param>
    public void StockPileFinish(StockPileStageData stockData)
    {
        SkillEnd();
        if (stockData.skillid == 0)
        {
            Debug.LogError("没有配置对应的蓄力技能id");
        }
        else
        {
            mSkillCreater.ReleaseSkill(stockData.skillid);
        }
    }


}
