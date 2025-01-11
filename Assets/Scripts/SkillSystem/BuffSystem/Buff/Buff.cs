using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;

public enum BuffState
{
    None,
    Delay,//延迟中
    Start,//开始
    Update,//Buff更新中
    End, //结束
}


public class Buff 
{
    /// <summary>
    /// buff配置
    /// </summary>
    public BuffConfig BuffCfg {  get; private set; }
    /// <summary>
    /// buff当前状态
    /// </summary>
    public BuffState buffState;


    /// <summary>
    /// buffID
    /// </summary>
    public readonly int Buffid;
    /// <summary>
    /// 释放者
    /// </summary>
    public LogicActor releaser;
    /// <summary>
    /// Bufff附加目标
    /// </summary>
    public LogicActor attachTarget;
    /// <summary>
    /// 隶属技能
    /// </summary>
    public Skill skill;
    /// <summary>
    /// Buff所需要的参数
    /// </summary>
    public object[] paramsObjs;

    /// <summary>
    /// 当前延迟时间
    /// </summary>
    private int mCurDelayTime;
    /// <summary>
    /// Buff逻辑组合对象
    /// </summary>
    private BuffComposite mBuffLogic;
    /// <summary>
    /// buff渲染对象
    /// </summary>
    //private BuffRender mBuffRender;
    /// <summary>
    /// 当前真实运行时间
    /// </summary>
    private int mCurRealRuntime;
    /// <summary>
    /// 当前累计运行时间
    /// </summary>
    private int mAccRumTime;

    public Buff(int buffid, LogicActor releaser, LogicActor attachTarget, Skill skill, object[] paramsObjs)
    {
        this.Buffid = buffid;
        this.releaser = releaser;
        this.attachTarget = attachTarget;
        this.skill = skill;
        this.paramsObjs = paramsObjs;
    }

    public void OnCreate()
    {
        // 加载配置
        BuffCfg = ZMAssetsFrame.LoadScriptableObject<BuffConfig>(AssetPathConfig.BUFF_DATA_PATH + Buffid + ".asset");

        if(BuffCfg.buffType == BuffType.Repel)
        {
            mBuffLogic = new RepelBuff(this);
        }
        else if(BuffCfg.buffType == BuffType.Floating)
        {
            mBuffLogic = new FloatingBuff(this);
        }

        buffState = BuffCfg.buffDelay == 0 ? BuffState.Start : BuffState.Delay;
        mCurDelayTime = BuffCfg.buffDelay;
    }

    public void OnLogicFrameUpdate()
    {
        switch (buffState)
        {
            case BuffState.Delay:
                if (mCurDelayTime == BuffCfg.buffDelay)
                {
                    //处理延时逻辑
                }
                mCurDelayTime -= LogicFrameConfig.LogicFrameIntervalMS;
                if(mCurDelayTime <= 0)
                {
                    buffState = BuffState.Start;
                }
                break;
            case BuffState.Start:
                //1.调用BuffStart的接口
                mBuffLogic.BuffStart();
                //2.调用Buff的触发接口
                mBuffLogic.BuffTrigger();
                //判断Buff是否需要切换为更新状态，如果当前Buff持续时间为有限或无限，才能进入更新状态
                buffState = (BuffCfg.buffDurationms == -1 || BuffCfg.buffDurationms > 0) ? BuffState.Update : BuffState.End;
                break;
            case BuffState.Update:
                UpdateBuffLogic();
                break;
            case BuffState.End:
                
                OnDestroy();
                break;
        }
    }

    public void UpdateBuffLogic()
    {

        int logicFrameIntervalMS = LogicFrameConfig.LogicFrameIntervalMS;
        //1.处理Buff间隔逻辑
        if(BuffCfg.buffIntervalms> 0)
        {
            //当前累计运行时间是否大于Buff间隔时间
            if(mCurRealRuntime >= BuffCfg.buffIntervalms)
            {
                //处理Buff逻辑
                mBuffLogic.BuffTrigger();
               mCurRealRuntime -= BuffCfg.buffIntervalms;
            }
            
        }

        //处理 当前Buff的持续时间
        UpdateBuffDurationTime();
    }

    public void UpdateBuffDurationTime()
    {
        mAccRumTime += LogicFrameConfig.LogicFrameIntervalMS;
        if(mAccRumTime >= BuffCfg.buffDurationms)
        {
            buffState = BuffState.End;
        }
    }



    public void OnDestroy()
    {
        mBuffLogic.BuffEnd();
        BuffSystem.Instance.RemoveBuff(this);
    }



}
