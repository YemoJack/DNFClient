using FixIntPhysics;
using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZMGC.Battle;

public enum DamageSource
{
    None,
    Skill,//技能伤害
    Buff,//Buff伤害
    Bullet,//子弹伤害
}


// SkillDamage
public partial class Skill
{
    
    /// <summary>
    /// 碰撞体字典
    /// </summary>
    private Dictionary<int,ColliderBehaviour> mColliderDic = new Dictionary<int,ColliderBehaviour>();
    /// <summary>
    /// 当前已累计的伤害时间
    /// </summary>
    private int mCurDamageAccTime;

    /// <summary>
    /// 逻辑帧更新伤害
    /// </summary>
    public void OnLogicFrameUpdateDamage()
    {
        //判断当前伤害配置列表是否为空，以及长度是否大于0
        if(mSkillData.damageCfgList != null && mSkillData.damageCfgList.Count > 0)
        {
            foreach(var item in mSkillData.damageCfgList)
            {

                int hashCode = item.GetHashCode();

                if (item.colliderPosType == ColliderPosType.FollowPos)
                {
                    ColliderBehaviour damageCollider = null;
                    //更新碰撞体位置
                    if (mColliderDic.TryGetValue(hashCode, out damageCollider))
                    {
                        CreateOrUpdateCollider(item, damageCollider);
                    }
                }
                //创建碰撞体
                if(mCurLogicFrame == item.triggerFrame)
                {
                    DestroyCollider(item);
                    ColliderBehaviour collider = CreateOrUpdateCollider(item,null);
                    //创建字典缓存当前碰撞体
                    mColliderDic.Add(hashCode, collider);

                    //处理碰撞体伤害检测
                    if (item.triggerIntervalMs == 0)
                    {
                        //触发一次伤害 //TODO
                        if (mColliderDic.ContainsKey(hashCode))
                        {
                            TriggerColliderDamage(mColliderDic[hashCode], item);
                        }
                    }
                }


                if (item.triggerIntervalMs != 0)
                {
                    //触发多次伤害
                    mCurDamageAccTime += item.triggerIntervalMs;
                    //如果当前累计时间大于触发伤害间隔 则进行伤害检测
                    if(mCurDamageAccTime>= item.triggerIntervalMs)
                    {
                        //触发一次伤害//TODO
                        mCurDamageAccTime = 0;
                        if (mColliderDic.ContainsKey(hashCode))
                        {
                            TriggerColliderDamage(mColliderDic[hashCode], item);
                        }
                    }
                }


                //销毁碰撞体
                if(item.endFrame == mCurLogicFrame)
                {
                    DestroyCollider(item);
                }
            }
        }


    }


    /// <summary>
    /// 触发碰撞体伤害
    /// </summary>
    public void TriggerColliderDamage(ColliderBehaviour collider,SkillDamageConfig damageConfig)
    {
        //1. 获取敌人目标列表 敌人 英雄
        List<LogicActor> enemyList = BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>().GetEnemyList(mSkillCreater.ObjectType);
        //2. 通过碰撞检测逻辑，去检测碰撞到的敌人
        List<LogicActor> damageTargetList = new List<LogicActor>();
        foreach(var enemy in enemyList)
        {
            if(collider.ColliderType == ColliderType.Box)
            {
                //如果返回值为true，则表示有碰撞到敌人，否则没有碰撞到敌人
                if(PhysicsManager.IsCollision(collider as FixIntBoxCollider, enemy.Collider))
                {
                    damageTargetList.Add(enemy);
                }
            }
            else if(collider.ColliderType == ColliderType.Shpere)
            {
                if(PhysicsManager.IsCollision( enemy.Collider, collider as FixIntSphereCollider))
                {
                    damageTargetList.Add(enemy);
                }
            }
        }
        //释放列表
        enemyList.Clear();

        //3. 获取到攻击目标后，对这些敌人造成伤害
        foreach(var target in damageTargetList)
        {
            //对敌人造成伤害
            target.SkillDamage(9999, damageConfig);

            //添加Buff TODO...

            //添加击中特效
            AddHitEffect(target);
            //添加击中音效
            PlayHitAudio();
        }
    }

    /// <summary>
    /// 添加击中特效
    /// </summary>
    /// <param name="targetObj"></param>
    public void AddHitEffect(LogicActor targetObj)
    {
        if(mSkillData.skillCfg.skillHitEffect != null)
        {
            targetObj.OnHit(mSkillData.skillCfg.skillHitEffect,mSkillData.skillCfg.hitEffectSurivalTimeMs,mSkillCreater);
        }
    }



    /// <summary>
    /// 创建碰撞体
    /// </summary>
    public ColliderBehaviour CreateOrUpdateCollider(SkillDamageConfig item , ColliderBehaviour damageCollider,LogicObject followobj = null)
    {
        ColliderBehaviour collider = damageCollider;
        LogicObject followTargetObj = followobj == null ? mSkillCreater : followobj;
        //创建对应的定点数碰撞体
        if(item.detectionMode == DamageDetectionMode.BOX3D)
        {
            FixIntVector3 boxSize = new FixIntVector3(item.boxSize);
            FixIntVector3 offset = new FixIntVector3(item.boxOffset) * followTargetObj.LogicXAxis;
            //限制y轴的偏移只能往上进行偏移
            offset.y = FixIntMath.Abs(offset.y);
            if(collider == null)
                collider = new FixIntBoxCollider(boxSize,offset);
            collider.SetBoxData(offset, boxSize);

            collider.UpdateColliderInfo(followTargetObj.LogicPos, boxSize);
        }
        else if(item.detectionMode == DamageDetectionMode.Sphere3D)
        {
            FixIntVector3 offset = new FixIntVector3(item.sphereOffset) * followTargetObj.LogicXAxis;
            //限制y轴的偏移只能往上进行偏移
            offset.y = FixIntMath.Abs(offset.y);
            if (collider == null)
                collider = new FixIntSphereCollider(item.radius, offset);
            collider.SetBoxData(item.radius, offset);
            collider.UpdateColliderInfo(followTargetObj.LogicPos, FixIntVector3.zero,item.radius);
        }

        return collider;
    }


    public void DestroyCollider(SkillDamageConfig item)
    {
        ColliderBehaviour collider = null;
        int HashCode = item.GetHashCode();
        mColliderDic.TryGetValue(HashCode, out collider);
        if (collider != null)
        {
            mColliderDic.Remove(HashCode);
            collider.OnRelease();
        }
    }

}
