using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移动类型
/// </summary>
public enum MoveType
{
    Target,
    X,
    Y,
    Z,
}




public class MoveToAction : ActionBehavior
{
    private LogicObject mActionObj;
    private FixIntVector3 mStartPos;
    private FixInt mMoveTime;
    private MoveType mMoveType;

    /// <summary>
    /// 移动向量
    /// </summary>
    private FixIntVector3 mMoveDistance;

    /// <summary>
    /// 当前累计运行的时间
    /// </summary>
    private FixInt mAccRunTime;

    /// <summary>
    /// 当前时间缩放
    /// </summary>
    private FixInt mTimeScale;


    public MoveToAction(LogicObject actionObj, FixIntVector3 startPos,FixIntVector3 targetPos, FixInt time,Action moveFinishCallBack,Action updateCallBack,MoveType moveType)
    {
        //接收参数
        mActionObj = actionObj;
        mStartPos = startPos;
        mMoveTime = time;
        mMoveType = moveType;
        mActionFinishCallBack = moveFinishCallBack;
        mUpdateActionCallBack = updateCallBack;
        mMoveDistance = targetPos - startPos;
    }




    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public override void OnLogicFrameUpdate()
    {
        //计算当前累计时间
        mAccRunTime += LogicFrameConfig.LogicFrameIntervalMS;
        //获取时间缩放比例
        mTimeScale = mAccRunTime / mMoveTime;

        if(mTimeScale >= 1)
        {
            mTimeScale = 1;
            actionFinish = true;
        }

        mUpdateActionCallBack?.Invoke();


        //计算对象需要移动的位置
        FixIntVector3 addDistance = FixIntVector3.zero; //添加的一个向量距离
        if(mMoveType == MoveType.Target)
        {
            addDistance = mMoveDistance * mTimeScale;
        }
        else if(mMoveType == MoveType.X)
        {
            addDistance.x = mMoveDistance.x * mTimeScale;
        }
        else if( mMoveType == MoveType.Y)
        {
            addDistance.y = mMoveDistance.y * mTimeScale;
        }
        else if(mMoveType == MoveType.Z)
        {
            addDistance.z = mMoveDistance.z * mTimeScale;
        }

        mActionObj.LogicPos = mStartPos + addDistance;

    }

    public override void OnActionFinish()
    {
        if(actionFinish)
        {
            mActionFinishCallBack?.Invoke();
        }
    }


}
