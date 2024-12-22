using FixIntPhysics;
using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectLogic : LogicObject
{

    private LogicActor mSkillCreater;
    private SkillEffectConfig mEffectCfg;

    private ColliderBehaviour mCollider;
    private float mAccRunTime; //累计运行时间


    public SkillEffectLogic(LogicObjectType objType,SkillEffectConfig effectCfg,RenderObject renderObj,LogicActor skillCreater)
    {
        this.mSkillCreater = skillCreater;
        ObjectType = objType;
        RenderObj = renderObj;
        mEffectCfg = effectCfg;
        LogicXAxis = mSkillCreater.LogicXAxis;
        //初始化特效逻辑位置
        if(effectCfg.effectPosType == EffectPosType.FollowPosDir || effectCfg.effectPosType == EffectPosType.FollowDir)
        {
            FixIntVector3 offset = new FixMath.FixIntVector3(effectCfg.effectOffsetPos) * LogicXAxis;
            offset.y = FixIntMath.Abs(offset.y);
            LogicPos = mSkillCreater.LogicPos + offset;
        }
        else if(effectCfg.effectPosType == EffectPosType.Zero)
        {
            LogicPos = FixIntVector3.zero;
        }
    }



    public void OnLogicFrameEffectUpdate(Skill skill,int curFrame)
    {
        
        if(mEffectCfg.effectPosType == EffectPosType.FollowPosDir)
        {
            FixIntVector3 offsetPos = new FixIntVector3(mEffectCfg.effectOffsetPos) * LogicXAxis;
            offsetPos.y = FixIntMath.Abs(offsetPos.y);
            LogicPos = mSkillCreater.LogicPos + offsetPos;
        }

        //1. 处理特效行动配置，让特效能够跟随配置移动
        if(mEffectCfg.isAttachAction && mEffectCfg.actionConfig.triggerFrame ==  curFrame)
        {
            skill.AddMoveAction(mEffectCfg.actionConfig, this, () =>{
                mCollider.OnRelease();
                skill.DestroyEffect(mEffectCfg);
                mCollider = null;
            }, () =>
            {
                //特效移动逻辑帧更新回调
                //更新碰撞体位置
                if (mEffectCfg.damageConfig.isFollowEffect)
                {
                    skill.CreateOrUpdateCollider(mEffectCfg.damageConfig, mCollider, this);
                }


                if (mEffectCfg.isAttachDamage)
                {
                    //处理间隔性伤害
                    if (mEffectCfg.damageConfig.triggerIntervalMs != 0 && mCollider != null)
                    {
                        mAccRunTime += LogicFrameConfig.LogicFrameIntervalMS;
                        if (mAccRunTime >= mEffectCfg.damageConfig.triggerIntervalMs)
                        {
                            skill.TriggerColliderDamage(mCollider, mEffectCfg.damageConfig);
                            mAccRunTime -= mEffectCfg.damageConfig.triggerIntervalMs;
                        }
                    }
                }
            });
        }
        //2. 处理伤害配置，让伤害碰撞体能够跟随动效进行移动
        if (mEffectCfg.isAttachDamage)
        {
            //创建伤害碰撞体
            if(mEffectCfg.damageConfig.triggerFrame == curFrame)
            {
                mCollider = skill.CreateOrUpdateCollider(mEffectCfg.damageConfig, null,this);
                if(mEffectCfg.damageConfig.triggerIntervalMs == 0)
                {
                    skill.TriggerColliderDamage(mCollider,mEffectCfg.damageConfig);
                }
            }
           

            //更新碰撞体位置
            if (mEffectCfg.damageConfig.isFollowEffect) 
            {
                skill.CreateOrUpdateCollider(mEffectCfg.damageConfig, mCollider, this);
            }


        }


    }


    public override void OnDestroy()
    {
        base.OnDestroy();
        RenderObj.OnRelease();
    }

}
