using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ա�����ƶ��߼��ű�
/// </summary>
public partial class LogicActor
{
    private FixIntVector3 mInputMoveDir;
    /// <summary>
    /// �߼�֡λ�ø���
    /// </summary>
    public void OnLogicFrameUpdateMove()
    {
        Collider?.UpdateColliderInfo(LogicPos, Collider.Size);
        if (ActionSate != LogicObjectActionState.Idle && ActionSate != LogicObjectActionState.Move && IsForceAllowMove==false)
        {
            return;
        }
        //�����߼�λ��
        LogicPos += mInputMoveDir* LogicMoveSpeed * (FixInt)LogicFrameConfig.LogicFrameInterval;

        //�����߼�����ĳ���
        if (LogicDir!=mInputMoveDir)
        {
            LogicDir = mInputMoveDir;
        }
        //�����߼�����
        if (LogicDir.x!=FixInt.Zero&& IsForceNotAlllowModifyDir == false)
        {
            LogicXAxis = LogicDir.x > 0 ? 1 : -1;
        }
        //Debug.Log("LogicPos:"+LogicPos);
     }

    public void InputLoigcFrameEvent(FixIntVector3 inputDir)
    {
        mInputMoveDir = inputDir;
    }
}
