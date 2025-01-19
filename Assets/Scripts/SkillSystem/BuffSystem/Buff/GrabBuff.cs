using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBuff : BuffComposite
{
    public GrabBuff(Buff buff) : base(buff) { }

    public override void BuffDelay()
    {

    }


    public override void BuffStart()
    {

    }

    public override void BuffTrigger()
    {
        //����ץȡ���ݣ��õ�ǰBuff���ӵ�Ŀ�����ָ����λ��

        //1.��������Ŀ���
        LogicObject attachTarget = mBuff.attachTarget;
        LogicObject releaser = mBuff.releaser;
        //2.ץȡ��������ɫ����λ��
        attachTarget.LogicPos = releaser.LogicPos;
        //3.�ѹ���ץȡ��ָ����Ŀ���
        FixIntVector3 grabPos = new FixIntVector3(mBuff.BuffCfg.targetGrabData.garbMoveTargetPos) * releaser.LogicXAxis;
        grabPos.y = FixIntMath.Abs(grabPos.y);
        //ץȡĿ��λ��
        FixIntVector3 targetGrabPos = releaser.LogicPos + grabPos;
        //�����ж�
        MoveToAction moveToAction = new MoveToAction(attachTarget,attachTarget.LogicPos,targetGrabPos,
            mBuff.BuffCfg.targetGrabData.moveTimems,null,null,MoveType.target);
        //ִ���ж�
        LogicActionController.Instance.RunAciton(moveToAction);
        Debug.Log("targetGrabPos:"+ targetGrabPos);
    }

    public override void BuffEnd()
    {
        Debug.Log("Grab End");
    }
}
