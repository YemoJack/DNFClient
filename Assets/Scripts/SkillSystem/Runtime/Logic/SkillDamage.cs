using FixIntPhysics;
using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZMGC.Battle;


// �˺��߼�
public partial class Skill
{
    
    /// <summary>
    /// ��ײ���ֵ�
    /// </summary>
    private Dictionary<int,ColliderBehaviour> mColliderDic = new Dictionary<int,ColliderBehaviour>();
    /// <summary>
    /// ��ǰ���ۼƵ��˺�ʱ��
    /// </summary>
    private int mCurDamageAccTime;

    /// <summary>
    /// �߼�֡�����˺�
    /// </summary>
    public void OnLogicFrameUpdateDamage()
    {
        //�жϵ�ǰ�˺������б��Ƿ�Ϊ�գ��Լ������Ƿ����0
        if(mSkillData.damageCfgList != null && mSkillData.damageCfgList.Count > 0)
        {
            foreach(var item in mSkillData.damageCfgList)
            {

                int hashCode = item.GetHashCode();
                //������ײ��
                if(mCurLogicFrame == item.triggerFrame)
                {
                    DestroyCollider(item);
                    ColliderBehaviour collider = CreateCollider(item);
                    //�����ֵ仺�浱ǰ��ײ��
                    mColliderDic.Add(hashCode, collider);
                }

                //������ײ���˺����
                if(item.triggerIntervalMs == 0)
                {
                    //����һ���˺� //TODO
                    if (mColliderDic.ContainsKey(hashCode))
                    {
                        TriggerColliderDamage(mColliderDic[hashCode]);
                    }
                }
                else
                {
                    //��������˺�
                    mCurDamageAccTime += item.triggerIntervalMs;
                    //�����ǰ�ۼ�ʱ����ڴ����˺���� ������˺����
                    if(mCurDamageAccTime>= item.triggerIntervalMs)
                    {
                        //����һ���˺�//TODO
                        mCurDamageAccTime = 0;
                        if (mColliderDic.ContainsKey(hashCode))
                        {
                            TriggerColliderDamage(mColliderDic[hashCode]);
                        }
                    }
                }


                //������ײ��
                if(item.endFrame == mCurLogicFrame)
                {
                    DestroyCollider(item);
                }
            }
        }


    }


    /// <summary>
    /// ������ײ���˺�
    /// </summary>
    public void TriggerColliderDamage(ColliderBehaviour collider)
    {
        //1. ��ȡ����Ŀ���б� ���� Ӣ��
        List<LogicActor> enemyList = BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>().GetEnemyList(mSkillCreater.ObjectType);
        //2. ͨ����ײ����߼���ȥ�����ײ���ĵ���
        List<LogicActor> damageTargetList = new List<LogicActor>();
        foreach(var enemy in enemyList)
        {
            if(collider.ColliderType == ColliderType.Box)
            {
                //�������ֵΪtrue�����ʾ����ײ�����ˣ�����û����ײ������
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
        //3. ��ȡ������Ŀ��󣬶���Щ��������˺�
    }



    /// <summary>
    /// ������ײ��
    /// </summary>
    public ColliderBehaviour CreateCollider(SkillDamageConfig item)
    {
        ColliderBehaviour collider = null;

        //������Ӧ�Ķ�������ײ��
        if(item.detectionMode == DamageDetectionMode.BOX3D)
        {
            FixIntVector3 boxSize = new FixIntVector3(item.boxSize);
            FixIntVector3 offset = new FixIntVector3(item.boxOffset) * mSkillCreater.LogicXAxis;
            //����y���ƫ��ֻ�����Ͻ���ƫ��
            offset.y = FixIntMath.Abs(boxSize.y);
            collider = new FixIntBoxCollider(boxSize,offset);
            collider.SetBoxData(offset, boxSize);

            collider.UpdateColliderInfo(mSkillCreater.LogicPos, boxSize);
        }
        else if(item.detectionMode == DamageDetectionMode.Sphere3D)
        {
            FixIntVector3 offset = new FixIntVector3(item.sphereOffset) * mSkillCreater.LogicXAxis;
            //����y���ƫ��ֻ�����Ͻ���ƫ��
            offset.y = FixIntMath.Abs(offset.y);

            collider = new FixIntSphereCollider(item.radius, offset);
            collider.SetBoxData(item.radius, offset);
            collider.UpdateColliderInfo(mSkillCreater.LogicPos, FixIntVector3.zero,item.radius);
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
