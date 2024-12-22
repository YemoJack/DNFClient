using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZMGC.Battle;


//LogicActorSkill
public partial class LogicActor
{

    /// <summary>
    /// 技能系统
    /// </summary>
    private SkillSystem mSkillSystem;


    /// <summary>
    /// 普通攻击技能id数组
    /// </summary>
    private int[] mNormalSkillidArr = new int[] { 1001,1002,1003};

    private int[] mSkillidArr;

    public List<Skill> releaseSkillList = new List<Skill>();

    /// <summary>
    /// 当前普通攻击连击索引
    /// </summary>
    private int mCurNormalComboIndex = 0;

    /// <summary>
    /// 初始化技能系统
    /// </summary>
    public virtual void InitActorSkill()
    {

        HeroDataMgr heroData = BattleWorld.GetExitsDataMgr<HeroDataMgr>();
        mNormalSkillidArr = heroData.GetHeroNormalSkillArr(1000);
        mSkillidArr = heroData.GetHeroSkillArr(1000);

        mSkillSystem = new SkillSystem(this);
        mSkillSystem.InitSkills(mNormalSkillidArr);
        mSkillSystem.InitSkills(mSkillidArr);
    }


    /// <summary>
    /// 释放普通攻击
    /// </summary>
    public void ReleaseNormalAttack()
    {
        ReleaseSkill(mNormalSkillidArr[mCurNormalComboIndex]);
    }

    /// <summary>
    /// 判断技能是否为普通攻击
    /// </summary>
    /// <param name="skillid"></param>
    /// <returns></returns>
    public bool IsNormalAttackSkill(int skillid)
    {
        foreach (var skill in mNormalSkillidArr)
        {
            if(skillid == skill)
                return true;
        }
        return false;
    }


    public Skill GetSKill(int skillid)
    {
        return mSkillSystem.GetSkill(skillid);
    }


    /// <summary>
    /// 释放技能
    /// </summary>
    /// <param name="skillid"></param>
    public virtual void ReleaseSkill(int skillid)
    {
        Skill skill = mSkillSystem.ReleaseSkill(skillid, OnSkillReleaseAfter, OnSkillReleaseEnd);
        if(skill != null)
        {
            releaseSkillList.Add(skill);
            if (!IsNormalAttackSkill(skillid))
            {
                mCurNormalComboIndex = 0;
            }

            ActionState = LogicObjectActionState.ReleaseSkill;
        }
    }

    /// <summary>
    /// 触发蓄力技能
    /// </summary>
    /// <param name="skillid"></param>
    public void TriggerStockPileSkill(int skillid)
    {
        mSkillSystem.TriggerStockPileSkill(skillid);
    }



    /// <summary>
    /// 释放技能后摇
    /// </summary>
    /// <param name="skill"></param>
    public virtual void OnSkillReleaseAfter(Skill skill)
    {
        //Debug.Log("释放技能后");
        if(!IsNormalAttackSkill(skill.skillid))
        {
            mCurNormalComboIndex = 0;
        }
        else
        {
            mCurNormalComboIndex++;
            //如果当前普攻攻击技能索引大于等于普通攻击技能数组长度，索引归零
            if(mCurNormalComboIndex >= mNormalSkillidArr.Length)
            {
                mCurNormalComboIndex = 0;
            }
        }
    }

    /// <summary>
    /// 释放技能结束
    /// </summary>
    /// <param name="skill"></param>
    public virtual void OnSkillReleaseEnd(Skill skill)
    {
        //Debug.Log("释放技能结束");
        releaseSkillList.Remove(skill);
        if(releaseSkillList.Count== 0)
        {
            ActionState = LogicObjectActionState.Idle;
            mCurNormalComboIndex = 0;
        }
    }


    /// <summary>
    /// 技能伤害(受击伤害)
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="damageConfig"></param>
    public virtual void SkillDamage(FixInt hp, SkillDamageConfig damageConfig)
    {
        Debug.Log($" SkillDamage  {hp}");
        CacluDamage(hp, DamageSource.Skill);
    }

    public virtual void OnHit(GameObject effectHitObj, int surcicalTimems,LogicActor source)
    {
        RenderObj.OnHit(effectHitObj, surcicalTimems, source);
    }



    /// <summary>
    /// 计算伤害
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="damageSource"></param>
    public virtual void CacluDamage(FixInt hp, DamageSource damageSource)
    {
        if(ObjectState == LogicObjectState.Survival)
        {
            //1. 对象逻辑层血量减少

            //2. 判断对象是否死亡， 如果死亡就处理死亡逻辑

            //3. 进行伤害飘字渲染
            RenderObj.Damage(hp.RawInt, damageSource);
        }


    }


    /// <summary>
    /// 逻辑帧更新技能接口
    /// </summary>
    public virtual void OnLogicFrameUpdateSkill()
    {
        mSkillSystem.OnLogicFrameUpdate();
    }

}
