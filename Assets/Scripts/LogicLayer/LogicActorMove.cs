using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//LogicActorMove
public partial class LogicActor
{

    private FixIntVector3 mInputMoveDir;

    /// <summary>
    /// 逻辑帧更新移动接口
    /// </summary>
    public void OnLogicFrameUpdateMove()
    {
        if(ActionState != LogicObjectActionState.Idle && ActionState != LogicObjectActionState.Move)
        {
            return;
        }
        //计算逻辑位置
        LogicPos += mInputMoveDir * LogicMoveSpeed * (FixInt)LogicFrameConfig.LogicFrameInterval;

        //计算逻辑对象的朝向
        if(LogicDir != mInputMoveDir )
        {
            LogicDir = mInputMoveDir;
        }
        //计算逻辑轴向
        if(LogicDir.x != FixInt.Zero)
        {
            LogicXAxis = LogicDir.x > 0 ? 1 : -1;
        }
    }


    public void InputLogicFrameEvent(FixIntVector3 logicDir)
    {
        mInputMoveDir = logicDir;
    }



}
