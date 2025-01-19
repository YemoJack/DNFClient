using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicTiemr : TimerBehaviour
{

    private FixInt mDelayTime;
    private int mLoopCount;

    private FixInt mCurLogicFrameAccTime;
    /// <summary>
    /// ������ʱ��
    /// </summary>
    private FixInt mTotalTime;
    public LogicTiemr(FixInt delayTime,Action timerCallBack,int loopCount=1) //1,2
    {
        this.mDelayTime = delayTime;
        this.mLoopCount = loopCount;
        this.mTotalTime = loopCount * delayTime;
        this.mTimerFinishCalllBack = timerCallBack;
    }
    /// <summary>
    /// �߼�֡����
    /// </summary>
    public override void OnLogicFrameUpdate()
    {
        //����ʱ���ۼ�
        mCurLogicFrameAccTime += LogicFrameConfig.LogicFrameInterval;
        if (mCurLogicFrameAccTime>=mDelayTime)
        {
            mTimerFinishCalllBack?.Invoke();
            mCurLogicFrameAccTime -= mDelayTime;
            mTotalTime -= mDelayTime;
            //���ѭ������<=1 ˵����ǰ��ʱ���������
            if (mLoopCount<=1|| mTotalTime<=0)
            {
                TimerFinsih = true;
                mTimerFinishCalllBack = null;
            }
        }

    }

    public override void OnTimerFinish()
    {
         
    }
}
