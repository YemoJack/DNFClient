using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �ƶ�����
/// </summary>
public enum MoveType
{
    target,
    X,
    Y,
    Z,
}

public class MoveToAction : ActionBehaviour
{
    private LogicObject mActionObj;
    private FixIntVector3 mStartPos;
    private FixInt mMoveTime;
    private MoveType mMoveType;
    /// <summary>
    /// �ƶ�������
    /// </summary>
    private FixIntVector3 mMoveDistance;
    /// <summary>
    /// ��ǰ�ۼ����е�ʱ��
    /// </summary>
    private FixInt mAccRumTime;
    /// <summary>
    /// ��ǰ�ƶ���ʱ������
    /// </summary>
    private FixInt mTimeScale;
    public MoveToAction(LogicObject actionObj,FixIntVector3 startPos,FixIntVector3 targerPos,
        FixInt time,Action moveFinsihCallBack,Action updateCallBack,MoveType moveType)
    {
        //���ղ���
        mActionObj = actionObj;
        mStartPos = startPos;
        mMoveTime = time==FixInt.Zero? 0.1f:time;
        mMoveType = moveType;
        mActionFinishCalllBack = moveFinsihCallBack;
        mUpdateActionCallBack = updateCallBack;
        //Ŀ��λ��-��ʼλ��=�ƶ��ƶ�����
        mMoveDistance = targerPos - startPos;
    }
    /// <summary>
    /// �ж����
    /// </summary>
    public override void OnActionFinish()
    {
        if (actionFinsih)
        {
            mActionFinishCalllBack?.Invoke();
        }
    }
    /// <summary>
    /// �߼�֡����
    /// </summary>
    public override void OnLogicFrameUpdate()
    {
        //���㵱ǰ�ۼ�����ʱ��
        mAccRumTime += LogicFrameConfig.LogicFrameIntervalms;
        //��ȡʱ�����ű���
        mTimeScale = mAccRumTime / mMoveTime;

        if (mTimeScale>=1)
        {
            mTimeScale = 1;
            actionFinsih = true;
        }
        mUpdateActionCallBack?.Invoke();
        //���������Ҫ�ƶ���λ��
        FixIntVector3 addDistance=FixIntVector3.zero;//��ӵ�һ����������
        if (mMoveType== MoveType.target)
        {
            addDistance = mMoveDistance * mTimeScale;
            mActionObj.LogicPos = mStartPos + addDistance;
        }
        else if (mMoveType== MoveType.X)
        {
            addDistance.x = mMoveDistance.x * mTimeScale;
            mActionObj.LogicPos = new FixIntVector3(mStartPos.x + addDistance.x,mActionObj.LogicPos.y,mActionObj.LogicPos.z);
        }
        else if (mMoveType == MoveType.Y)
        {
            addDistance.y =  mMoveDistance.y * mTimeScale;
            mActionObj.LogicPos = new FixIntVector3(mActionObj.LogicPos.x, mStartPos.y + addDistance.y, mActionObj.LogicPos.z);
        }
        else if (mMoveType == MoveType.Z)
        {
            addDistance.z = mMoveDistance.z * mTimeScale;
            mActionObj.LogicPos = new FixIntVector3(mActionObj.LogicPos.x,mActionObj.LogicPos.y, mStartPos.z + addDistance.z);
        }
       
    }
     

}
