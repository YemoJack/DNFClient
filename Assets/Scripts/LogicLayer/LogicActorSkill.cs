using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZMGC.Battle;
using FixMath;
public partial class LogicActor
{
    /// <summary>
    /// ����ϵͳ
    /// </summary>
    private SkillSystem mSkillSystem;
    /// <summary>
    /// ��ͨ��������id����
    /// </summary>
    private int[] mNormalSkillidArr = new int[] { 1001, 1002, 1003 };

    private int[] mSkillidArr;

    /// <summary>
    /// �����ͷż��ܵ��б�
    /// </summary>
    public List<Skill> releaseingSkillList = new List<Skill>();
    /// <summary>
    /// ��ǰ��ͨ������������
    /// </summary>
    private int mCurNormalComboIndex = 0;
    /// <summary>
    /// ��ǰ������е�����buff
    /// </summary>
    private List<Buff> mBuffList = new List<Buff>();
    /// <summary>
    /// ��ʼ�������б�
    /// </summary>
    public void InitActorSkill(int id)
    {
        HeroDataMgr heroData = BattleWorld.GetExitsDataMgr<HeroDataMgr>();
        mNormalSkillidArr = heroData.GetHeroNormalSkilidArr(id);
        mSkillidArr = heroData.GetHeroSkillIdArr(id);
        mSkillSystem = new SkillSystem(this);
        mSkillSystem.InitSKills(mNormalSkillidArr);
        mSkillSystem.InitSKills(mSkillidArr);
    }
    /// <summary>
    /// �ͷ���ͨ����
    /// </summary>
    public void ReleaseNormalAttack()
    {
        ReleaseSKill(mNormalSkillidArr[mCurNormalComboIndex]);
    }
    /// <summary>
    /// �ͷŶ�Ӧ�ļ���
    /// </summary>
    /// <param name="skillid"></param>
    public void ReleaseSKill(int skillid,FixIntVector3 guidePos=default(FixIntVector3), Action<bool> releaseSkillCallBack = null)
    {
        Skill skill = mSkillSystem.ReleaseSkill(skillid, guidePos,   OnSkillReleaseAfter, (skill)=> {
            if (skill.SKillCfg.skillType== SKillType.StockPile)
            {
                releaseSkillCallBack?.Invoke(true);
            }
            OnSkillReleaseEnd(skill);
        });
        //��=null ˵�������ͷųɹ�
        if (skill != null)
        {
            releaseingSkillList.Add(skill);
            if (!IsNormalAttackSkill(skill.skillid))
            {
                mCurNormalComboIndex = 0;
            }
            ActionSate = LogicObjectActionState.ReleasingSkill;
            if (skill.SKillCfg.skillType!= SKillType.StockPile)
            {
                releaseSkillCallBack?.Invoke(true);
            }
        }
        else
        {
            releaseSkillCallBack?.Invoke(false);
        }
    }
    /// <summary>
    /// ����������������
    /// </summary>
    /// <param name="skillid"></param>
    public void TriggerStockPileSkill(int skillid)
    {
        mSkillSystem.TriggerStockPileSkill(skillid);
    }
    /// <summary>
    /// �Ƿ�����ͨ��������
    /// </summary>
    /// <param name="skillid">У��ļ���id</param>
    /// <returns></returns>
    public bool IsNormalAttackSkill(int skillid)
    {
        foreach (var item in mNormalSkillidArr)
        {
            if (skillid == item)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// �����ͷź�ҡ
    /// </summary>
    /// <param name="skill"></param>
    public void OnSkillReleaseAfter(Skill skill)
    {
        if (!IsNormalAttackSkill(skill.skillid))
        {
            mCurNormalComboIndex = 0;
        }
        else
        {
            mCurNormalComboIndex++;
            //�����ǰ��ͨ�����������Դ��ڵȼ���ͨ�����������鳤�ȣ�������0
            if (mCurNormalComboIndex >= mNormalSkillidArr.Length || skill.skillid == mNormalSkillidArr[mNormalSkillidArr.Length - 1])
            {
                mCurNormalComboIndex = 0;
            }
        }
    }
    /// <summary>
    /// �����ͷ����
    /// </summary>
    /// <param name="skill"></param>
    public void OnSkillReleaseEnd(Skill skill)
    {
        releaseingSkillList.Remove(skill);
        if (releaseingSkillList.Count == 0)
        {
            ActionSate = LogicObjectActionState.Idle;
            mCurNormalComboIndex = 0;
        }
    }

    public Skill GetSKill(int skillid)
    {
        return mSkillSystem.GetSKill(skillid);
    }
    /// <summary>
    /// �߼�֡���¼��ܽӿ�
    /// </summary>
    public void OnLogicFrameUpdateSkill()
    {
        if (mSkillSystem==null)
        {
            return;
        }
        mSkillSystem?.OnLogicFrameUpdate();
    }
    /// <summary>
    /// ���һ��buff
    /// </summary>
    /// <param name="buff"></param>
    public void AddBuff(Buff buff)
    {
        mBuffList.Add(buff);
    }
    /// <summary>
    /// �Ƴ�һ��buff
    /// </summary>
    /// <param name="buff"></param>
    public void RemoveBuff(Buff buff)
    {
        if (mBuffList.Contains(buff))
        {
            mBuffList.Remove(buff);
        }
        if (ObjectState == LogicObjectState.Death)
        {
            return;
        }

        if (mBuffList.Count == 0 && RenderObj.GetCurAnimName() != AnimationName.Anim_Getup)
        {
            PlayAnim(AnimationName.Anim_Idle);
            ActionSate = LogicObjectActionState.Idle;
        }
    }
}
