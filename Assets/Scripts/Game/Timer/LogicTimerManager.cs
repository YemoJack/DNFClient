using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;
public class LogicTimerManager : Singleton<LogicTimerManager>
{
    /// <summary>
    /// ��ʱ���б�
    /// </summary>
    private List<LogicTiemr> mTimerList = new List<LogicTiemr>();

    /// <summary>
    /// ��ʼ�����ж�
    /// </summary>
    /// <param name="action"></param>
    public void DelayCall(FixInt delayTime,Action timerCallBack,int loopCount=1)
    {
        LogicTiemr timer = new LogicTiemr(delayTime,timerCallBack,loopCount);
        mTimerList.Add(timer);
    }
    /// <summary>
    /// �߼�֡����֡
    /// </summary>
    public void OnLogicFrameUpdate()
    {
        //�Ƴ��Ѿ���ɵ��ж�
        for (int i = mTimerList.Count-1; i >=0 ; i--)
        {
            LogicTiemr timer = mTimerList[i];
            if (timer.TimerFinsih)
            {
                RemoveTimer(timer);
            }
        }
        //�����߼�֡
        foreach (var item in mTimerList)
        {
            item.OnLogicFrameUpdate();
        }
    }
    /// <summary>
    /// �Ƴ���Ӧ���ж�
    /// </summary>
    /// <param name="action"></param>
    public void RemoveTimer(LogicTiemr timer)
    {
        mTimerList.Remove(timer);
    }
    /// <summary>
    /// �ű���Դ�ͷ�
    /// </summary>
    public void OnDestroy()
    {
        mTimerList.Clear();
    }
}
