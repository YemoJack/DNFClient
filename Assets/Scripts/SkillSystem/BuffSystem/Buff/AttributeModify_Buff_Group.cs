using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeModify_Buff_Group : BuffComposite
{
    /// <summary>
    /// �����˺�ֵ
    /// </summary>
    private FixInt configValue;
    /// <summary>
    /// ��ǰbuff��Ӧ����ײ��
    /// </summary>
    private BuffCollider mBuffCollider;
    public AttributeModify_Buff_Group(Buff buff) : base(buff) { }


    public override void BuffDelay()
    {

    }
    public override void BuffStart()
    {
        //��ȡ���ò���
        if (mBuff.BuffCfg.buffParamsList.Count>0)
            configValue = mBuff.BuffCfg.buffParamsList[0].value;

        if (mBuff.BuffCfg.targetConfig.isOpen)
        {
            //ʹ�����ģʽ�����ɶ�Ӧ����ײ�� 
            mBuffCollider = new BuffCollider(mBuff);
            mBuffCollider.CreateOrUpdateCollider();
        }

    }
    public override void BuffTrigger()
    {
        //�����ײ����ײ��������Ŀ��
        if (mBuff.BuffCfg.targetConfig.isOpen)
        {
            //��ȡ��ǰ��ײ����ײ�������е�Ŀ��
            List<LogicActor> targetList = mBuffCollider.CacleColliderTargetObjects();
            for (int i = 0; i < targetList.Count; i++)
            {
                //��ȡBuff����Ŀ��
                LogicActor target = targetList[i];
                if (target.ObjectState!= LogicObjectState.Death)
                {
                    //����˺�
                    target.BuffDamage(DamageCalcuCenter.CaclulateDamage(mBuff.BuffCfg, mBuff.releaser, target), mBuff.BuffCfg.targetConfig.damageCfg);
                    target.OnHit(mBuff.BuffCfg.buffHitEffectObjPath,1,mBuff.releaser,mBuff.releaser.LogicXAxis);
                    //��������˺����BUff�ĸ���
                    int[] buffidArr = mBuff.BuffCfg.targetConfig.damageCfg.addBuffs;
                    if (buffidArr!=null&&buffidArr.Length>0)
                    {
                        for (int k = 0; k < buffidArr.Length; k++)
                        {
                            BuffSystem.Instance.AttachBuff(buffidArr[k],mBuff.releaser,target,mBuff.skill);
                        }
                    }
                }
            }
            targetList.Clear();
            targetList = null;
        }
    }
    public override void BuffEnd()
    {
        //���ٵ�ǰ��ײ��
        mBuffCollider?.OnRelease();
        mBuffCollider = null;
    }
}
