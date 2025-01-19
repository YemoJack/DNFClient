using FixIntPhysics;
using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZMGC.Battle;
public class SKillBulletLogic : LogicObject
{
    /// <summary>
    /// �ӵ���������
    /// </summary>
    private Skill mSKill;
    /// <summary>
    /// �ӵ�������
    /// </summary>
    private LogicActor mFireLogicActor;
    /// <summary>
    /// �ӵ�����
    /// </summary>
    private SkillBulletConfig mBulletCfg;

    /// <summary>
    /// ��ǰ�ӵ���ײ��
    /// </summary>
    private ColliderBehaviour mBulletCollider;
    //��ǰ�߼�֡
    private int mCurLogicFrame = 0;
    //��ǰ�߼�֡�ۼ�ʱ��
    private int mCurLogicFrameAccTime;
    //�ӵ��Ƿ����Ŀ��
    private bool mBulletIsHit=false;
    //�Ƿ�ʧЧ
    public bool isFailure = false;   
    //�ӵ���ǰ֡����Ŀ���б�
    private List<LogicActor> mHitTargetList = new List<LogicActor>();
    public SKillBulletLogic(Skill skill, LogicActor fireLogicActor, RenderObject renderObj, SkillBulletConfig bulletCfg, FixIntVector3 rangePos)
    {
        mSKill = skill;
        mFireLogicActor = fireLogicActor;
        RenderObj = renderObj;
        mBulletCfg = bulletCfg;
        //֡ͬ���д����������ô����  1.����ʹ�ô�������� ��Ӧ��ʹ�û���������ӵ�α�����
        //ʲô�� ������� PPT
        //��ʼ������λ��
        //��������

        LogicXAxis = fireLogicActor.LogicXAxis;

        //��ʼ���߼�������
        LogicDir = new FixIntVector3(LogicXAxis,0,0)+new FixIntVector3(bulletCfg.dir);
         //��ʼ����ǰ����ƫ��λ��
        FixIntVector3 pos = LogicXAxis * (new FixIntVector3(mBulletCfg.offset) + rangePos);
        pos.y= FixIntMath.Abs(pos.y);
        LogicPos = fireLogicActor.LogicPos + pos;

        //������ת�Ƕ�
        LogicAngle = new FixIntVector3(bulletCfg.angle) * LogicXAxis;
        
        //��ǰ����ӵ��Ƿ񸽼��˺�
        if (bulletCfg.isAttachDamage)
        {
            SkillDamageConfig damageCfg = bulletCfg.damageCfg;
            if (damageCfg.detectionMode== DamageDetectionMode.BOX3D)
            {
                mBulletCollider = new FixIntBoxCollider(FixIntVector3.zero,FixIntVector3.zero);
            }
            else if (damageCfg.detectionMode == DamageDetectionMode.Sphere3D)
            {
                mBulletCollider = new FixIntSphereCollider(damageCfg.raduis, FixIntVector3.zero);
            }
        }
    }
    /// <summary>
    /// �����ӵ���ײ���ӵ��ƶ��߼�
    /// </summary>
    public override void OnLogicFrameUpdate()
    {
        base.OnLogicFrameUpdate();
        //�����߼�֡�ۼ�ʱ��
        mCurLogicFrameAccTime = mCurLogicFrame * LogicFrameConfig.LogicFrameIntervalms;
        //�߼�֡����
        mCurLogicFrame++;
       
        //�ӵ������߼�
        foreach (LogicActor target in mHitTargetList)
        {
      
            //����ӵ��˺�
            target.BulletDamage(DamageCalcuCenter.CaclulateDamage(mBulletCfg.damageCfg, mFireLogicActor,target),mBulletCfg.damageCfg);
            //���Ż���Ч��
            target.OnHit(mBulletCfg.hitEffectPath ,mBulletCfg.hitEffectSurvivalTimems,this,LogicXAxis);
            //���������Ч
            if (mBulletCfg.hitAudio!=null)
            {
                AudioController.GetInstance().PlaySoundByAudioClip(mBulletCfg.hitAudio,false,1);
            }
            //�����ӵ����ӵ�Buff
            AttachBuff(target);

            if (mBulletCfg.isHitDestory)
            {
                Release();
                break;
            }
        }
        //����Ŀ�괦����ɣ�����������
        if (mHitTargetList.Count>0)
        {
            mHitTargetList.Clear();
        }
        //�ӵ���ײ��λ�ø���
        if (mBulletCollider != null)
        {
            if (mBulletCfg.damageCfg.colliderPosType== ColliderPosType.FollowPos)
            {
                //�����ӵ���ײ��λ��
                if (mBulletCfg.damageCfg.detectionMode== DamageDetectionMode.BOX3D)
                {
                    FixIntVector3 offset = LogicXAxis * new FixIntVector3(mBulletCfg.damageCfg.boxOffset);
                    mBulletCollider.SetBoxData(offset,new FixIntVector3(mBulletCfg.damageCfg.boxSize));
                    mBulletCollider.UpdateColliderInfo(LogicPos, new FixIntVector3(mBulletCfg.damageCfg.boxSize));
                }
                else if (mBulletCfg.damageCfg.detectionMode == DamageDetectionMode.Sphere3D)
                {
                    FixIntVector3 offset = LogicXAxis * new FixIntVector3(mBulletCfg.damageCfg.sphereOffset);
                    mBulletCollider.SetBoxData(mBulletCfg.damageCfg.raduis, offset);
                    mBulletCollider.UpdateColliderInfo(LogicPos,FixIntVector3.zero, mBulletCfg.damageCfg.raduis);
                }
            }

            //��ȡ���������еĵ���
            List<LogicActor> enemyList= BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>().GetEnemyList(mFireLogicActor.ObjectType);
            //�����ӵ���ײ���Ƿ���е���
            foreach (var target in enemyList)
            {
                if (mBulletCfg.damageCfg.detectionMode == DamageDetectionMode.BOX3D)
                {
                   mBulletIsHit= PhysicsManager.IsCollision(mBulletCollider as FixIntBoxCollider, target.Collider);
                }
                else if (mBulletCfg.damageCfg.detectionMode == DamageDetectionMode.Sphere3D)
                {
                    mBulletIsHit = PhysicsManager.IsCollision(target.Collider, mBulletCollider as FixIntSphereCollider);
                }
                //�ѻ��е�Ŀ��������б�
                if (mBulletIsHit)
                {
                    mHitTargetList.Add(target);
                }
            }
        }

        //�ӵ�λ�ø���
        LogicPos += LogicDir * (FixInt)mBulletCfg.moveSpeed * (FixInt)LogicFrameConfig.LogicFrameInterval;
        //��ǰ����ʱ�䣬�ﵽ���ӵ����������ӵ�
        if (mCurLogicFrameAccTime>=mBulletCfg.survivalTimeMsg)
        {
            Release();
        }
    }
    /// <summary>
    /// �����ӵ�����buff
    /// </summary>
    public void AttachBuff(LogicActor target)
    {

        if (mBulletCfg.damageCfg.addBuffs!=null&& mBulletCfg.damageCfg.addBuffs.Length>0)
        {
            foreach (var buffid in mBulletCfg.damageCfg.addBuffs)
            {
                BuffSystem.Instance.AttachBuff(buffid,mFireLogicActor, target,mSKill);
            }
        }
    }
    public void Release()
    {
        RenderObj.OnRelease();
        mBulletCollider?.OnRelease();
        mBulletCollider = null;
        isFailure = true;
    }
}
