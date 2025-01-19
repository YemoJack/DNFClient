using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeModify_Buff_Single : BuffComposite
{
    /// <summary>
    /// �����˺�ֵ
    /// </summary>
    private FixInt configValue;

    public AttributeModify_Buff_Single(Buff buff) : base(buff) { }


    public override void BuffDelay()
    {

    }
    public override void BuffStart()
    {
        //��ȡ���ò���
        if (mBuff.BuffCfg.buffParamsList.Count > 0)
            configValue = mBuff.BuffCfg.buffParamsList[0].value;
    }
    public override void BuffTrigger()
    {
        ModifyAttribute(configValue);
    }
    /// <summary>
    /// �޸�����
    /// </summary>
    /// <param name="value"></param>
    public void ModifyAttribute(FixInt value)
    {
        switch (mBuff.BuffCfg.buffType)
        {
            case BuffType.MoveSpeed_Modify_Single:
                mBuff.attachTarget.LogicMoveSpeed += value;
                break;
        }

    }
    public override void BuffEnd()
    {
        ModifyAttribute(-configValue);
    }
}