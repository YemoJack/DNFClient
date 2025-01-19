using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LogicActor : LogicObject
{
    public override void OnCreate()
    {
        base.OnCreate();
    }
 
    public override void OnLogicFrameUpdate()
    {
        base.OnLogicFrameUpdate();
        //�����ƶ�֡
        OnLogicFrameUpdateMove();
        //���¼���֡
        OnLogicFrameUpdateSkill();
        //��������֡
        OnLogicFrameUpdateGravity();
        //�����ӵ�֡
        OnLogicFramUpdateBullet();
    }

    public void PlayAnim(AnimationClip clip)
    {
        RenderObj.PlayAnim(clip);
    }
    public void PlayAnim(string name)
    {
        RenderObj.PlayAnim(name);
    }
    public virtual void OnHit(string effectHitObjPath,int survivalTimems, LogicObject source,FixInt logicXAxis)
    {
        RenderObj.OnHit(effectHitObjPath, survivalTimems, source);
    }
    public virtual void SkillDamage(FixInt hp,SkillDamageConfig damageConfig)
    {
        Debug.Log("SkillDamage hp:"+hp);
        CacluDamage(hp, DamageSource.SKill);
    }

    public virtual void BuffDamage(FixInt hp, SkillDamageConfig damageConfig)
    {
        Debug.Log("BuffDamage hp:" + hp);
        CacluDamage(hp, DamageSource.SKill);
    }
    /// <summary>
    /// ĳ�����ܻ�buff����ٻ��赲�ӵ��˺�
    /// </summary>
    public virtual void BulletDamage(FixInt hp,SkillDamageConfig damageConfig)
    {
        Debug.Log("BulletDamage hp:" + hp);
        CacluDamage(hp, DamageSource.Bullet);
    }


    /// <summary>
    /// �����˺�
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="source"></param>
    public void CacluDamage(FixInt hp,DamageSource source)
    {
        if (ObjectState== LogicObjectState.Survival)
        {
            //1.�����߼���Ѫ������
            ReduceHP(hp);
            //2.�ж϶����Ƿ����� ��������ʹ��������߼�
            if (this.HP<=0)
            {
                Collider.Active = false;
                ObjectState = LogicObjectState.Death;
                RenderObj.OnDeath();
            }
            //3.�����˺�Ʈ����Ⱦ
            RenderObj.Damage(hp.RawInt, source);
        }
    }

    /// <summary>
    /// ���ջص���
    /// </summary>
    /// <param name="uploating">�Ƿ����ϸ�</param>
    public virtual void Floating(bool upfoating) {}
    /// <summary>
    /// ���󴥵�
    /// </summary>
    /// <param name="upfoating"></param>
    public virtual void TriggerGround() { }
    public override void OnDestroy()
    {
        base.OnDestroy();

    }
}
