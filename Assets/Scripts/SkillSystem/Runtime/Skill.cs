using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;

public enum SkillState
{
    None,
    Befor,//����ǰҡ
    After,//���ܺ�ҡ
    End,//���ܽ���
}

public partial class Skill
{

    public int skillid;
    /// <summary>
    /// ���ܴ�����
    /// </summary>
    public LogicActor mSkillCreater;
    /// <summary>
    /// ������������
    /// </summary>
    private SkillDataConfig mSkillData;

    public SkillConfig SKillCfg { get { return mSkillData.skillCfg; } }
    /// <summary>
    /// �˺������б�
    /// </summary>
    public List<SkillDamageConfig> damageCfgList { get { return mSkillData.damageCfgList; } }
     /// <summary>
    /// �ͷż��ܺ�ҡ
    /// </summary>
    public Action<Skill> OnReleaseAfter;
    /// <summary>
    /// �ͷż��ܽ����ص�
    /// </summary>
    public Action<Skill, bool> OnReleaseSkillEnd;
    /// <summary>
    /// ����״̬
    /// </summary>
    public SkillState skillState = SkillState.None;
    /// <summary>
    /// ��ǰ�߼�֡
    /// </summary>
    private int mCurLogicFrame = 0;
    /// <summary>
    /// ��ǰ�ۼ�����ʱ��
    /// </summary>
    private int mCurLogicFrameAccTime = 0;
    /// <summary>
    /// �Ƿ��Զ�ƥ�������׶�
    /// </summary>
    private bool mAutoMacthStockStage;

    /// <summary>
    /// ��������λ��
    /// </summary>
    public FixIntVector3 sKillGuidePos;
    /// <summary>
    /// ��ϼ���id
    /// </summary>
    private int mCombinationSkillid;
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="skillid">����id</param>
    /// <param name="skillCreater">���ܴ�����</param>
    public Skill(int skillid, LogicActor skillCreater)
    {
        this.skillid = skillid;
        this.mSkillCreater = skillCreater;
        mSkillData = ZMAssetsFrame.LoadScriptableObject<SkillDataConfig>(AssetPathConfig.SKILL_DATA_PATH + skillid + ".asset");
    }
    /// <summary>
    /// �ͷż���
    /// </summary>
    /// <param name="releaseAfterCallBack">���ܺ�ҡ</param>
    /// <param name="releaseSkillEnd">�ͷż��ܽ���</param>
    public void ReleaseSKill(Action<Skill> releaseAfterCallBack, FixIntVector3 guidePos, Action<Skill, bool> releaseSkillEnd)
    {
        OnReleaseAfter = releaseAfterCallBack;
        OnReleaseSkillEnd = releaseSkillEnd;
        sKillGuidePos = guidePos;
        SkillStart();
        skillState = SkillState.Befor;
        PlayAnim();
    }
    /// <summary>
    /// ���ż��ܶ���
    /// </summary>
    public void PlayAnim()
    {
        //���Ž�ɫ����
        mSkillCreater.PlayAnim(mSkillData.character.skillAnim);
    }
    /// <summary>
    /// ����ǰҡ
    /// </summary>
    public void SkillStart()
    {
        //��ʼ�ͷż���ʱ����ʼ����������
        mCurLogicFrame = 0;
        mCurLogicFrameAccTime = 0;
        mAutoMacthStockStage = false;
        mCombinationSkillid = mSkillData.skillCfg.ComobinationSkillid;
         if (mSkillData.character.customLogicFame != 0)
            mSkillData.character.logicFrame = mSkillData.character.customLogicFame;
        OnBulletInit();
        OnInitDamage();
    }
    /// <summary>
    /// ���ܺ�ҡ
    /// </summary>
    public void SkillAfter()
    {
        skillState = SkillState.After;
        OnReleaseAfter?.Invoke(this);
    }
    /// <summary>
    /// �����ͷŽ���
    /// </summary>
    public void SKillEnd()
    {
        skillState = SkillState.End;
        OnReleaseSkillEnd?.Invoke(this, mSkillData.skillCfg.ComobinationSkillid != 0);
        ReleaseAllEffect();
        OnBulletRelease();
        OnDamageRelease();
        //����Ƿ�����ϼ���
        if (mCombinationSkillid != 0)
        {
            mSkillCreater.ReleaseSKill(mCombinationSkillid);
        }
    }

    /// <summary>
    /// �߼�֡����
    /// </summary>
    public void OnLogicFrameUpdate()
    {
        if (skillState == SkillState.None||skillState== SkillState.End)
        {
            return;
        }
        //�����ۼ�����ʱ��
        mCurLogicFrameAccTime = mCurLogicFrame * LogicFrameConfig.LogicFrameIntervalms;

        //�����ܺ�ҡ
        if (skillState == SkillState.Befor && mCurLogicFrameAccTime >= mSkillData.skillCfg.skillShakeArfterMs&&mSkillData.skillCfg.skillType!= SKillType.StockPile)
        {
            SkillAfter();
        }

        //���²�ͬ���õ��߼�֡������ͬ���õ��߼�

        //������Ч�߼�֡
        OnLogicFrameUpdateEffect();
        //�����˺��߼�֡
        OnLogicFrameUpdateDamage();
        //�����ж��߼�֡
        OnLogicFrameUpdateAction();
        //������Ч�߼�֡
        OnLogicFrameUpdateAudio();
        //�����ӵ��߼�֡
        OnLogicFrameUpdateBullet();
        //����Buff�߼�֡
        OnLogicFrameUpdateBuff();
        
        //��Ϊ����������Ҫͨ������ʱ����д��������Ժͼ��ܵĽ���֡�޹�
        if (mSkillData.skillCfg.skillType == SKillType.StockPile)
        {
            int stockDataCount = mSkillData.skillCfg.stockPileStageData.Count;
            if (stockDataCount > 0)
            {
                //1.������ָ��������̧���һ�����
                if (mAutoMacthStockStage)
                {
                    //�Զ�ƥ���һ�׶��������ܽ����ͷ�
                    StockPileStageData stockData = mSkillData.skillCfg.stockPileStageData[0];
                    if (mCurLogicFrameAccTime >= stockData.startTimeMs)
                    {
                        StockPileFinish(stockData);
                    }
                }
                else
                {
                    //2.����ʱ�������߼�
                    StockPileStageData stockData = mSkillData.skillCfg.stockPileStageData[stockDataCount - 1];
                    //��������ʱ���Ƿ�ﵽ���ֵ������ﵽ���ֵ���Զ�������������׶μ���(�����û���ָ�����������ܰ�ť����)
                    if (mCurLogicFrameAccTime >= stockData.endTimeMs)
                    {
                        StockPileFinish(stockData);
                    }
                }
            }
        }
        else
        {
            //�жϼ����Ƿ��ͷŽ���
            if (mCurLogicFrame == mSkillData.character.logicFrame)
            {
                SKillEnd();
            }
        }

        //����������������
        if (mSkillData.skillCfg.showSkillPortrait && mCurLogicFrame==0)
        {
            mSkillCreater.RenderObj.ShowSkillPortrait(mSkillData.skillCfg.skillProtraitObj);
        }
        //�߼�֡����
        mCurLogicFrame++;
    }
    /// <summary>
    /// ����������������
    /// </summary>
    public void TriggerStockPileSkill()
    {
        //1.����ʱ����������׶������е�ĳһ������
        foreach (var item in mSkillData.skillCfg.stockPileStageData)
        {
            if (mCurLogicFrameAccTime>=item.startTimeMs&&mCurLogicFrameAccTime<=item.endTimeMs)
            {
                StockPileFinish(item);
                return;
            }
        }
        //2.����ʱ����̣�����������������׶μ��ܣ��Զ�������һ�׶���������
        mAutoMacthStockStage = true;
    }
    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="stockData"></param>
    public void StockPileFinish(StockPileStageData stockData)
    {
        SKillEnd();
        if (stockData.skillid == 0)
        {
            Debug.LogError("���������ͷ�ʧ�ܣ������׶μ���idΪ0");
        }
        else
        {
            mSkillCreater.ReleaseSKill(stockData.skillid);
        }
    }
}
