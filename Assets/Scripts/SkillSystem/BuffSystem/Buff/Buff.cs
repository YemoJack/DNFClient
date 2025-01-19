using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;
public enum BuffState
{
    None,
    Delay,//�ӳ���
    Start,//��ʼ����
    Update,//Buff������
    End,//buff����
}

public class Buff
{
    /// <summary>
    /// buff����
    /// </summary>
    public BuffConfig BuffCfg { get; private set; }
    /// <summary>
    /// buff��ǰ״̬
    /// </summary>
    public BuffState buffState;
    /// <summary>
    /// BuffΨһid
    /// </summary>
    public readonly int Buffid;
    /// <summary>
    /// buff�ͷ���
    /// </summary>
    public LogicActor releaser;
    /// <summary>
    /// buff����/����Ŀ��
    /// </summary>
    public LogicActor attachTarget;
    /// <summary>
    /// ��������
    /// </summary>
    public Skill skill;
    /// <summary>
    /// Buff����Ҫ��һЩ����
    /// </summary>
    public object[] paramsObjs;
    /// <summary>
    /// ��ǰ�ӳ�ʱ��
    /// </summary>
    private int mCurDelayTime;
    /// <summary>
    /// Buff�߼���϶���
    /// </summary>
    private BuffComposite mBuffLogic;
    /// <summary>
    /// buff��Ⱦ����
    /// </summary>
    private BuffRender mBuffRender;
    /// <summary>
    /// ��ǰ��ʵ����ʱ��
    /// </summary>
    private int mCurRealRuntime;
    /// <summary>
    /// ��ǰ�ۼ�����ʱ��
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
        //����buff�����ļ�
        BuffCfg = ZMAssetsFrame.LoadScriptableObject<BuffConfig>(AssetPathConfig.BUFF_DATA_PATH + Buffid + ".asset");
        if (BuffCfg.buffType == BuffType.Repel)
        {
            mBuffLogic = new RepelBuff(this);
        }
        else if (BuffCfg.buffType == BuffType.Floating)
        {
            mBuffLogic = new FloatingBuff(this);
        }
        else if (BuffCfg.buffType == BuffType.Stiff)
        {
            mBuffLogic = new StiffBuff(this);
        }
        else if (BuffCfg.buffType == BuffType.HP_Modify_Group)
        {
            mBuffLogic = new AttributeModify_Buff_Group(this);
        }
        else if (BuffCfg.buffType == BuffType.Grab)
        {
            mBuffLogic = new GrabBuff(this);
        }
        else if (BuffCfg.buffType == BuffType.IgnoreGravity)
        {
            mBuffLogic = new IgnoreGravityBuff(this);
        }
        else if (BuffCfg.buffType == BuffType.MoveSpeed_Modify_Single)
        {
            mBuffLogic = new AttributeModify_Buff_Single(this);
        }
        else if (BuffCfg.buffType == BuffType.AllowMove|| BuffCfg.buffType == BuffType.NotAllowDir)
        {
            mBuffLogic = new StatusModify_Buff_Single(this);
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
                    //����BUff�ӳ��߼�
                    mBuffLogic.BuffDelay();
                }
                mCurDelayTime -= LogicFrameConfig.LogicFrameIntervalms;
                if (mCurDelayTime <= 0)
                {
                    buffState = BuffState.Start;
                }
                break;
            case BuffState.Start:
                BuffStart();
                BuffTrigger();
                //�ж�buff�Ƿ���Ҫ�л�Ϊ����״̬�������ǰbuff����ʱ��Ϊ���޻����ޣ��Ž������״̬
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
    public void BuffStart()
    {
        CreateBuffEffect();
        mBuffRender?.InitBuffRender(releaser, attachTarget, BuffCfg, skill.sKillGuidePos);
        //1.����buffStart�ӿ�
        mBuffLogic.BuffStart();
        attachTarget.AddBuff(this);
    }
    public void BuffTrigger()
    {
        mBuffLogic.BuffTrigger();
        switch (BuffCfg.buffTriggerAnim)
        {
            case ObjectAnimationState.BeHit:
                attachTarget.PlayAnim(AnimationName.Anim_Beiji_01);
                break;
            case ObjectAnimationState.Stiff:
                attachTarget.PlayAnim(AnimationName.Anim_Beiji_02);
                break;
        }
        //����ǰBUff��Ҫ���ŵ���Ч
        //if (BuffCfg.buffAudio != null)
        //{
        //    AudioController.GetInstance().PlaySoundByAudioClip(BuffCfg.buffAudio, false, 2);
        //}
    }
    public void UpdateBuffLogic()
    {
        int logicFrameintervalMs = LogicFrameConfig.LogicFrameIntervalms;
        //1.����buff����߼�
        if (BuffCfg.buffIntervalms > 0)
        {
            //��ǰ�ۼ�����ʱ���Ƿ����buff���������������ھʹ���buffЧ��
            mCurRealRuntime += logicFrameintervalMs;
            if (mCurRealRuntime >= BuffCfg.buffIntervalms)
            {
                BuffTrigger();
                mCurRealRuntime -= BuffCfg.buffIntervalms;
            }
        }
        //����ǰBuff�ĳ���ʱ��
        UpdateBuffDurationTime();
    }
    public void UpdateBuffDurationTime()
    {
        mAccRumTime += LogicFrameConfig.LogicFrameIntervalms;
        if (mAccRumTime >= BuffCfg.buffDurationms)
        {
            buffState = BuffState.End;
        }
    }
    /// <summary>
    /// ����buff��Ч
    /// </summary>
    public BuffRender CreateBuffEffect()
    {
        //��ȡBuffEffect���ã�ȥ���ɶ�Ӧ����Ч
        if (BuffCfg.effectConfig != null && BuffCfg.effectConfig.effect!=null)
        {
            //GameObject buffEffect = GameObject.Instantiate(BuffCfg.effectConfig.effect);
            GameObject buffEffect =ZM.AssetFrameWork.ZMAssetsFrame.Instantiate(BuffCfg.effectConfig.effectPath,null);
            mBuffRender = buffEffect.GetComponent<BuffRender>();
            if (mBuffRender == null)
            {
                mBuffRender = buffEffect.AddComponent<BuffRender>();
            }
            return mBuffRender;
        }
        return null;
    }
    public void OnDestroy()
    {
        mBuffRender?.OnRelease();
        mBuffLogic.BuffEnd();
        BuffSystem.Instance.RemoveBuff(this);
        attachTarget.RemoveBuff(this);
    }
}
