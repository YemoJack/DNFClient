using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixMath;
using FixIntPhysics;
public class SkillEffectLogic : LogicObject
{
    private LogicActor mSkillCreater;
    private SkillEffectConfig mEffectCfg;
    private ColliderBehaviour mCollider;
    private int mAccRumtime;//�ۼ�����ʱ��
    public SkillEffectLogic(LogicObjectType objType, SkillEffectConfig effectCfg, RenderObject renderObject, LogicActor skillCreater,Skill skill)
    {
        this.ObjectType = objType;
        this.RenderObj = renderObject;
        this.mSkillCreater = skillCreater;
        this.mEffectCfg = effectCfg;
        this.LogicXAxis = skillCreater.LogicXAxis;
        //��ʼ����Ч�߼�λ��
        if (effectCfg.effectPosType == EffectPosType.FollowDir || effectCfg.effectPosType == EffectPosType.FollowPosDir)
        {
            FixIntVector3 offsetPos = new FixIntVector3(effectCfg.effectOffsetPos) * LogicXAxis;
            offsetPos.y = FixIntMath.Abs(offsetPos.y);
            LogicPos = skillCreater.LogicPos + offsetPos;
        }
        else if (effectCfg.effectPosType == EffectPosType.Zero)
        {
            LogicPos = FixIntVector3.zero;
        }
        else if (effectCfg.effectPosType== EffectPosType.GuidePos)
        {
            //��ʼλ��
            FixIntVector3 initPos= skill.sKillGuidePos + mSkillCreater.LogicXAxis * new FixIntVector3(effectCfg.effectOffsetPos);
            initPos.y = FixIntMath.Abs(initPos.y);
            LogicPos = initPos;
        }
 
    }

    public void OnLogicFrameEffectUpdate(Skill skill, int curLogicFrame)
    {
        if (mEffectCfg.effectPosType == EffectPosType.FollowPosDir)
        {
            FixIntVector3 offsetPos = new FixIntVector3(mEffectCfg.effectOffsetPos) * LogicXAxis;
            offsetPos.y = FixIntMath.Abs(offsetPos.y);
            LogicPos = mSkillCreater.LogicPos + offsetPos;
        }
        //1.������Ч�ж����ã�����Ч�ܹ����������ƶ�
        if (mEffectCfg.isAttachAction && mEffectCfg.actionConfig.triggerFrame == curLogicFrame)
        {
            skill.AddMoveAction(mEffectCfg.actionConfig, this,mEffectCfg.effectOffsetPos, () =>
            {
                Debug.Log("MoveToAction Finish SkillEffectLogic");
                mCollider?.OnRelease();
                skill.DestroyEffect(mEffectCfg);
                mCollider = null;
            }, () =>
            {
                //��Ч�ƶ��߼�֡�ص�
                //������ײ��λ��
                if (mEffectCfg.damageConfig.isFollowEffect)
                {
                    skill.CreateOrUpdateCollider(mEffectCfg.damageConfig, mCollider, this);
                }
                if (mEffectCfg.isAttachDamage)
                {
                    //���������˺�
                    if (mEffectCfg.damageConfig.triggerIntervalMs != 0 && mCollider != null)
                    {
                        mAccRumtime += LogicFrameConfig.LogicFrameIntervalms;
                        if (mAccRumtime >= mEffectCfg.damageConfig.triggerIntervalMs)
                        {
                            skill.TriggerColliderDamage(mCollider, mEffectCfg.damageConfig);
                            mAccRumtime -= mEffectCfg.damageConfig.triggerIntervalMs;
                        }
                    }
                }
            });
        }
        //2.�����˺����ã����˺���ײ���ܹ����涯Ч�����ƶ�
        if (mEffectCfg.isAttachDamage)
        {
            //�����˺���ײ��
            if (mEffectCfg.damageConfig.triggerFrame == curLogicFrame)
            {
                mCollider = skill.CreateOrUpdateCollider(mEffectCfg.damageConfig, null, this);
                if (mEffectCfg.damageConfig.triggerIntervalMs == 0)
                {
                    skill.TriggerColliderDamage(mCollider, mEffectCfg.damageConfig);
                }
            }


        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        RenderObj.OnRelease();
    }
}
