using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//SkillAction
public partial class Skill
{
   
    /// <summary>
    /// 行动逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdateAction()
    {
        if(mSkillData.actionCfgList != null && mSkillData.actionCfgList.Count > 0)
        {
            foreach(var item  in mSkillData.actionCfgList)
            {
                if(item.triggerFrame == mCurLogicFrame)
                {
                    //触发行动
                    AddMoveAction(item, mSkillCreater);
                }
            }
        }
    }


    /// <summary>
    /// 添加移动行动
    /// </summary>
    /// <param name="item">行动配置</param>
    /// <param name="logicMoveObj">逻辑移动对象</param>
    public void AddMoveAction(SkillActionConfig item,LogicObject logicMoveObj,Action moveFinish = null)
    {
        FixIntVector3 movePos = new FixIntVector3(item.movePos);
        FixIntVector3 targetPos;
        targetPos = logicMoveObj.LogicPos + movePos * logicMoveObj.LogicXAxis;

        MoveType moveType = MoveType.Target;
        if(movePos.x != FixInt.Zero && movePos.y ==  FixInt.Zero && movePos.z == FixInt.Zero)
        {
            moveType = MoveType.X;
        }
        else if(movePos.x == FixInt.Zero && movePos.y != FixInt.Zero && movePos.z  == FixInt.Zero)
        {
            moveType = MoveType.Y;
        }
        else if(movePos.x == FixInt.Zero && movePos.y == FixInt.Zero && movePos.z != FixInt.Zero)
        {
            moveType = MoveType.Z;
        }

        //构建行动类
        MoveToAction action = new MoveToAction(logicMoveObj, logicMoveObj.LogicPos, targetPos, item.durationMs, () =>
        {
            moveFinish?.Invoke();
            if(item.actionFinishOpation != MoveActionFinishOpation.None)
            {
                switch(item.actionFinishOpation)
                {
                    case MoveActionFinishOpation.Skill:
                        foreach(var item in item.actionFinishidList)
                        {
                            mSkillCreater.ReleaseSkill(item);
                        }
                        break;
                    case MoveActionFinishOpation.Buff:
                        //TODO 
                        break;
                }
            }
        }, null, moveType);
        //开始行动
        LogicActionController.Instance.RunAction(action);

    }


}
