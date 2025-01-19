using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixIntPhysics;
using FixMath;
using ZMGC.Battle;

/// <summary>
/// �˺���Դ
/// </summary>
public enum DamageSource
{
    None,
    SKill,//�����˺�
    Buff,//Buff�˺�
    Bullet,//�ӵ��˺�
}
public partial class Skill
{
    /// <summary>
    /// ��Ч�����ֵ� keyΪ��Ч���õ�HashCode��ValueΪ���ɵĶ�Ӧ����Ч
    /// </summary>

    private Dictionary<int, ColliderBehaviour> mColliderDic = new Dictionary<int, ColliderBehaviour>();
    ///// <summary>
    ///// ��ǰ�˺��ۼ�ʱ��
    ///// </summary>
    //private int mCurDamageAccTime;

    //��ǰ�����ӵ��ۼ�ʱ���б�
    private List<int> mCurDamageAccTimeList = new List<int>();

    private void OnInitDamage()
    {
        if (mSkillData.damageCfgList != null && mSkillData.damageCfgList.Count > 0)
        {
            for (int i = 0; i < mSkillData.damageCfgList.Count; i++)
            {
                mCurDamageAccTimeList.Add(0);
            }
        }
    }
    /// <summary>
    /// �߼�֡�����˺�
    /// </summary>
    public void OnLogicFrameUpdateDamage()
    {
        //�жϵ�ǰ�˺������б��Ƿ�Ϊ�գ��Լ������Ƿ����0
        if (mSkillData.damageCfgList != null && mSkillData.damageCfgList.Count > 0)
        {
            for (int i = 0; i < mSkillData.damageCfgList.Count; i++)
            {
                SkillDamageConfig item = mSkillData.damageCfgList[i];
                int hashCode = item.GetHashCode();
                if (item.colliderPosType == ColliderPosType.FollowPos)
                {
                    ColliderBehaviour damageCollider = null;
                    //������ײ��λ��
                    if (mColliderDic.TryGetValue(item.GetHashCode(), out damageCollider) && damageCollider != null)
                    {
                        CreateOrUpdateCollider(item, damageCollider);
                    }
                }

                //������ײ��
                if (mCurLogicFrame == item.triggerFrame)
                {
                    DestroyCollider(item);
                    ColliderBehaviour collider = CreateOrUpdateCollider(item, null);
                    //�����ֵ仺�浱ǰ��ײ��
                    mColliderDic.Add(hashCode, collider);
                    if (item.triggerIntervalMs == 0)
                    {
                        //����һ���˺�//TOOD
                        if (mColliderDic.ContainsKey(hashCode))
                        {
                            TriggerColliderDamage(mColliderDic[hashCode], item);
                        }
                    }
                }

                //������ײ���˺����
                if (item.triggerIntervalMs != 0)
                {
                    //int mCurDamageAccTime = mCurDamageAccTimeList[i]; ֵ��������mCurDamageAccTimeList[i]��Ӧ��һ��ֵ
                    mCurDamageAccTimeList[i] += LogicFrameConfig.LogicFrameIntervalms;
                    //�����ǰ�ۼ�ʱ����ڴ����˺�������Ǿ�����˺����
                    if (mCurDamageAccTimeList[i] >= item.triggerIntervalMs)
                    {
                        //����һ���˺�//TOOD
                        mCurDamageAccTimeList[i] = 0;
                        if (mColliderDic.ContainsKey(hashCode))
                        {
                            TriggerColliderDamage(mColliderDic[hashCode], item);
                        }
                    }
                }

                //������ײ��
                if (item.endFrame == mCurLogicFrame)
                {
                    DestroyCollider(item);
                }
            }
        }
    }
    /// <summary>
    /// ������ײ���˺�
    /// </summary>
    public void TriggerColliderDamage(ColliderBehaviour collider, SkillDamageConfig config)
    {
        //1.��ȡ����Ŀ���б� ���� Ӣ��
        List<LogicActor> enemyList = BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>().GetEnemyList(mSkillCreater.ObjectType);

        //2.ͨ����ײ����߼���ȥ�����ײ���ĵ���
        List<LogicActor> damageTargetList = new List<LogicActor>();
        foreach (var target in enemyList)
        {
            if (collider.ColliderType == ColliderType.Box)
            {
                //�������ֵΪTrue��˵��������ײ�巢������ײ
                if (PhysicsManager.IsCollision(collider as FixIntBoxCollider, target.Collider))
                {
                    damageTargetList.Add(target);
                }
            }
            else if (collider.ColliderType == ColliderType.Shpere)
            {
                //�������ֵΪTrue��˵��������ײ�巢������ײ
                if (PhysicsManager.IsCollision(target.Collider, collider as FixIntSphereCollider))
                {
                    damageTargetList.Add(target);
                }
            }
        }
        //�ͷ��б�
        enemyList.Clear();
        //3.��ȡ������Ŀ��󣬶���Щ��������˺�
        foreach (var target in damageTargetList)
        {
            //����˺�
            target.SkillDamage(DamageCalcuCenter.CaclulateDamage(config,mSkillCreater,target), config);

            //���Buff 
            if (config.addBuffs != null && config.addBuffs.Length > 0)
            {
                foreach (var buffid in config.addBuffs)
                {
                    BuffSystem.Instance.AttachBuff(buffid, mSkillCreater, target, this, null);
                }
            }
            //������Ӧ�ĺ������� 
            if (config.triggerSkillid != 0)
            {
                //Ԥ�ͷż��� ������ܻ��ڵ�ǰ�����ͷ���ɺ� ���������ͷ�
                mCombinationSkillid = config.triggerSkillid;
            }
            //��ӻ�����Ч
            AddHitEffect(target, config.targetType == TargetType.Self ? mSkillCreater : target);
            //���Ż�����Ч
            PlayHitAudio();

        }
    }
    /// <summary>
    /// ��ӻ�����Ч
    /// </summary>
    public void AddHitEffect(LogicActor targetObj, LogicActor source)
    {
        if (mSkillData.skillCfg.skillHitEffect != null)
        {
            targetObj.OnHit(mSkillData.skillCfg.skillHitEffectPath, mSkillData.skillCfg.hitEffectSurvivalTimeMs, source, mSkillCreater.LogicXAxis);
        }
    }
    /// <summary>
    /// ������ײ��
    /// </summary>
    public ColliderBehaviour CreateOrUpdateCollider(SkillDamageConfig item, ColliderBehaviour damageCollider, LogicObject followObj = null)
    {
        ColliderBehaviour collider = damageCollider;
        LogicObject followTragetObj = followObj == null ? mSkillCreater : followObj;
        //������Ӧ�Ķ�������ײ��
        if (item.detectionMode == DamageDetectionMode.BOX3D)
        {
            FixIntVector3 boxSize = new FixIntVector3(item.boxSize);
            FixIntVector3 offset = new FixIntVector3(item.boxOffset) * followTragetObj.LogicXAxis;

            //����y���ƫ��ֻ�����Ͻ���ƫ��
            offset.y = FixIntMath.Abs(offset.y);
            if (damageCollider == null)
                collider = new FixIntBoxCollider(boxSize, offset);

            collider.SetBoxData(offset, boxSize);
            collider.UpdateColliderInfo(followTragetObj.LogicPos, boxSize);
        }
        else if (item.detectionMode == DamageDetectionMode.Sphere3D)
        {
            FixIntVector3 offset = new FixIntVector3(item.sphereOffset) * followTragetObj.LogicXAxis;
            //����y���ƫ��ֻ�����Ͻ���ƫ��
            offset.y = FixIntMath.Abs(offset.y);

            if (damageCollider == null)
                collider = new FixIntSphereCollider(item.raduis, offset);

            collider.SetBoxData(item.raduis, offset);
            collider.UpdateColliderInfo(followTragetObj.LogicPos, FixIntVector3.zero, item.raduis);
        }
        return collider;
    }



    /// <summary>
    /// ���ٶ�Ӧ�������ɵ���ײ��
    /// </summary>
    /// <param name="item"></param>
    public void DestroyCollider(SkillDamageConfig item)
    {
        ColliderBehaviour collider = null;
        int hashCode = item.GetHashCode();
        mColliderDic.TryGetValue(hashCode, out collider);
        if (collider != null)
        {
            mColliderDic.Remove(hashCode);
            collider.OnRelease();
        }
    }

    public void OnDamageRelease()
    {
        mCurDamageAccTimeList.Clear();
    }
}
