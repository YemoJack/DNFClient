using FixIntPhysics;
using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZMGC.Battle;

/// <summary>
/// ֻ����buff��ײ������ɡ���Ŀ��ļ�⣬������buff���˺�����߼���
/// ֻ�����ṩ��ײ�����API
/// </summary>
public class BuffCollider  
{
    /// <summary>
    /// ��ǰ��ײ��ʵ��
    /// </summary>
    private ColliderBehaviour mBuffCollider;
    /// <summary>
    /// Buff����
    /// </summary>
    private BuffConfig mBuffCfg;
    /// <summary>
    /// Buff�˺�����
    /// </summary>
    private SkillDamageConfig mDamageCfg;

    /// <summary>
    /// Buff�ͷ���
    /// </summary>
    private LogicActor mReleaser;

    /// <summary>
    /// Buff����Ŀ��
    /// </summary>
    private LogicActor mAttachTarget;
    /// <summary>
    /// ��ǰBuff��������
    /// </summary>
    private Skill mSKill;

    //��ʼ����ײ����������
    public BuffCollider(Buff buff)
    {
        mBuffCfg = buff.BuffCfg;
        mDamageCfg = buff.BuffCfg.targetConfig.damageCfg;
        mReleaser = buff.releaser;
        mAttachTarget = buff.attachTarget;
        mSKill = buff.skill;
    }

    //1.���ɶ�Ӧ����ײ��

    /// <summary>
    /// ������ײ��
    /// </summary>
    public ColliderBehaviour CreateOrUpdateCollider(LogicObject followObj = null)
    {
        //������Ӧ�Ķ�������ײ��
        if (mDamageCfg.detectionMode == DamageDetectionMode.BOX3D)
        {
            FixIntVector3 boxSize = new FixIntVector3(mDamageCfg.boxSize);
            FixIntVector3 offset = new FixIntVector3(mDamageCfg.boxOffset) ;

            //����y���ƫ��ֻ�����Ͻ���ƫ��
            offset.y = FixIntMath.Abs(offset.y);
            if (mBuffCollider == null)
                mBuffCollider = new FixIntBoxCollider(boxSize, offset);

            mBuffCollider.SetBoxData(offset, boxSize);
            mBuffCollider.UpdateColliderInfo(GetBuffPos(), boxSize);
        }
        else if (mDamageCfg.detectionMode == DamageDetectionMode.Sphere3D)
        {
            FixIntVector3 offset = new FixIntVector3(mDamageCfg.sphereOffset);
            //����y���ƫ��ֻ�����Ͻ���ƫ��
            offset.y = FixIntMath.Abs(offset.y);

            if (mBuffCollider == null)
                mBuffCollider = new FixIntSphereCollider(mDamageCfg.raduis, offset);

            mBuffCollider.SetBoxData(mDamageCfg.raduis, offset);
            mBuffCollider.UpdateColliderInfo(GetBuffPos(), FixIntVector3.zero, mDamageCfg.raduis);
        }
        return mBuffCollider;
    }



    //2.������ײ�壬����ж���Ŀ�귢������ײ
    public List<LogicActor> CacleColliderTargetObjects()
    {
        //1.��ȡ����Ŀ���б� ���� Ӣ��
        List<LogicActor> enemyList = BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>().GetEnemyList(mReleaser.ObjectType);

        //2.ͨ����ײ����߼���ȥ�����ײ���ĵ���
        List<LogicActor> damageTargetList = new List<LogicActor>();
        foreach (var target in enemyList)
        {
            if (mBuffCollider.ColliderType == ColliderType.Box)
            {
                //�������ֵΪTrue��˵��������ײ�巢������ײ
                if (PhysicsManager.IsCollision(mBuffCollider as FixIntBoxCollider, target.Collider))
                {
                    damageTargetList.Add(target);
                }
            }
            else if (mBuffCollider.ColliderType == ColliderType.Shpere)
            {
                //�������ֵΪTrue��˵��������ײ�巢������ײ
                if (PhysicsManager.IsCollision(target.Collider, mBuffCollider as FixIntSphereCollider))
                {
                    damageTargetList.Add(target);
                }
            }
        }
        return damageTargetList;
    }
    /// <summary>
    /// ��ȡBuff����λ��
    /// </summary>
    /// <returns></returns>
    public FixIntVector3 GetBuffPos()
    {
        if (mBuffCfg.attachType== BuffAttachType.Guide_Pos)
        {
            return mSKill.sKillGuidePos;
        }
        else if (mBuffCfg.attachType == BuffAttachType.Creator)
        {
            return mReleaser.LogicPos;
        }
        else if (mBuffCfg.attachType == BuffAttachType.Target)
        {
            return mAttachTarget.LogicPos;
        }
        else
        {
            return mReleaser.LogicPos;
        }
    }

    //3.�ͷŵ�ǰ��ײ��
    public void OnRelease()
    {
        mBuffCollider?.OnRelease();
        mBuffCollider = null;
    }
}
