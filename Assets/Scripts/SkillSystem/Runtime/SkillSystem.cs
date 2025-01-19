using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ϵͳ
/// </summary>
public class SkillSystem
{
    /// <summary>
    /// ����ϵͳ������
    /// </summary>
    private LogicActor mSkillCreater;
    /// <summary>
    /// ����id����
    /// </summary>
    private List<Skill> mSkillArr = new List<Skill>();
    /// <summary>
    /// ��ǰ�����ͷ��еļ���
    /// </summary>
    private Skill mCurReleasingSkill;
    /// <summary>
    /// ������ϼ���id�б�
    /// </summary>
    private List<int> mCombinationSkillIdList = new List<int>();
    public SkillSystem(LogicActor logicActor)
    {
        mSkillCreater = logicActor;
    }
    /// <summary>
    /// ��ʼ������
    /// </summary>
    /// <param name="skillidArr">����id����</param>
    public void InitSKills(int[] skillidArr)//1000 1001 1002
    {
        foreach (var skillid in skillidArr)
        {
            Skill skill = new Skill(skillid, mSkillCreater);
            mSkillArr.Add(skill);
            if (skill.SKillCfg.ComobinationSkillid != 0)
            {
                //ʹ�õݹ鷽ʽ���ü�����ϼ����м��ܵĳ�ʼ��
                InitSKills(new int[] { skill.SKillCfg.ComobinationSkillid });
            }
            //��ʼ��������������
            if (skill.SKillCfg.stockPileStageData.Count>0)
            {
                foreach (var item in skill.SKillCfg.stockPileStageData)
                {
                    InitSKills(new int[] { item.skillid });
                }
            }
            //��ʼ��������������
            if (skill.damageCfgList.Count > 0)
            {
                foreach (var item in skill.damageCfgList)
                {
                    if (item.triggerSkillid!=0)
                        InitSKills(new int[] { item.triggerSkillid });
                }
            }
        }
        Debug.Log("���ܳ�ʼ����� ���ܸ�����" + skillidArr.Length);
    }
    public Skill ReleaseSkill(int skillid, FixIntVector3 guidePos,  Action<Skill> releaseAfterCallBack, Action<Skill> releaseSkillEnd )
    {
        //�����ǰ�ļ��ܲ�Ϊ�գ��뵱ǰ�ļ��ܴ���ǰҡ���߸տ�ʼ�ͷŵ�һ��״̬�����ʱ���ǲ������ͷ��������ܵ�
        if (mCurReleasingSkill!=null&&(mCurReleasingSkill.skillState!= SkillState.End&&mCurReleasingSkill.skillState!= SkillState.After))
        {
            return null;
        }
        //��ϼ������ͷ��У�����ϼ��޷��ͷ�
        if (mCombinationSkillIdList.Count>0&&!mCombinationSkillIdList.Contains(skillid))
        {
            return null;
        }
        foreach (var skill in mSkillArr)
        {
            if (skill.skillid == skillid)
            {
                if (skill.skillState != SkillState.None && skill.skillState != SkillState.End)
                {
                    Debug.Log($"��ǰ���������ͷ��� skillID{skill} .�������ڴ��ͷţ�");
                    return null;
                }
                //�жϵ�ǰ�����Ƿ�����ϼ�
                if (skill.SKillCfg.ComobinationSkillid!=0)
                {
                    CacleteCombinationSkillIdList(skill.SKillCfg.ComobinationSkillid);
                }
                //�ͷż���
                skill.ReleaseSKill(releaseAfterCallBack , guidePos, (skill, combinationSkill) =>
                {
                    //�����ͷ���ɻص�
                    releaseSkillEnd?.Invoke(skill);
                    //�����ǰ��������ϼ��ܣ�����������߼�
                    if (!combinationSkill)
                    {
                        //�����ͷ���� ����˵����ϼ��ܵ����һ��
                        mCurReleasingSkill = null;
                        if (skill.SKillCfg.ComobinationSkillid==0&&mCombinationSkillIdList.Count>0)
                        {
                            mCombinationSkillIdList.Clear();
                        }
                    }
                });
                mCurReleasingSkill = skill;
                return skill;
            }
        }
        Debug.LogError("SKillid :" + skillid + " ���ܲ����ڣ����ñ���û���ҵ�");
        return null;
    }
    /// <summary>
    /// ����������������
    /// </summary>
    /// <param name="skillid"></param>
    public void TriggerStockPileSkill(int skillid)
    {
        //�����ǰ�ļ��ܲ�Ϊ�գ��뵱ǰ�ļ��ܴ���ǰҡ���߸տ�ʼ�ͷŵ�һ��״̬�����ʱ���ǲ������ͷ��������ܵ�
        if (mCurReleasingSkill != null && mCurReleasingSkill.skillid!=skillid)
        {
            return;
        }
        //�жϵ�ǰ�Ƿ�����ϼ������ͷ��У��������ϼ������ͷ��У��ǲ������ͷ��������ܵġ�
        //��ϼ������ͷ��У�����ϼ��޷��ͷ�
        if (mCombinationSkillIdList.Count > 0 && !mCombinationSkillIdList.Contains(skillid))
        {
            return;
        }
        Skill skill = GetSKill(skillid);
        if (skill != null)
        {
            skill.TriggerStockPileSkill();
        }
    }
    public Skill GetSKill(int skillid)
    {
        foreach (var item in mSkillArr)
        {
            if (item.skillid == skillid)
            {
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// ������ϼ���id�б�
    /// </summary>
    /// <param name="skillid"></param>
    public void CacleteCombinationSkillIdList(int skillid) //1000,1001 ,1002
    {
        if (skillid!=0)
        {
            int combinationSkillId = skillid;
            while (combinationSkillId!=0)
            {
                mCombinationSkillIdList.Add(combinationSkillId);
                combinationSkillId= GetSKill(combinationSkillId).SKillCfg.ComobinationSkillid;
            }

        }
        else
        {
            Debug.LogError("��Ч�ļ���id:"+skillid);
        }
    }
    public void OnLogicFrameUpdate()
    {
        foreach (var item in mSkillArr)
        {
            item.OnLogicFrameUpdate();
        }
    }
}
