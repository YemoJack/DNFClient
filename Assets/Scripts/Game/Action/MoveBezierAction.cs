using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBezierAction : ActionBehaviour
{
    private LogicObject mActionObj;
    private FixIntVector3 mStartPos;
    private FixIntVector3 mHeightPos;
    private FixIntVector3 mEndPos;
    private FixInt mMoveTime;
    /// <summary>
    /// ��ǰ�ۼ����е�ʱ��
    /// </summary>
    private FixInt mAccRumTime;
    /// <summary>
    /// ��ǰ�ƶ���ʱ������
    /// </summary>
    private FixInt mTimeScale;
    public MoveBezierAction(LogicObject actionObj, FixIntVector3 startPos, FixIntVector3 heightPos, FixIntVector3 endPos, FixInt time, Action moveFinsihCallBack, Action updateCallBack)
    {
        Debug.Log($"startPos:{startPos} heightPos:{heightPos} endPos:{endPos}");
        //���ղ���
        mActionObj = actionObj;
        mStartPos = startPos;
        mHeightPos = heightPos;
        mEndPos = endPos;
        mMoveTime = time == FixInt.Zero ? 0.1f : time;
        mActionFinishCalllBack = moveFinsihCallBack;
        mUpdateActionCallBack = updateCallBack;
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

        if (mTimeScale >= 1)
        {
            mTimeScale = 1;
            actionFinsih = true;
        }
        mUpdateActionCallBack?.Invoke();
        //���������Ҫ�ƶ���λ��
        mActionObj.LogicPos = BezierUtils.BezierCurve(mStartPos,mHeightPos,mEndPos, mTimeScale);

    }


}