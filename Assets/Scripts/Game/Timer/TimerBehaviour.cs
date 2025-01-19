using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimerBehaviour
{
    /// <summary>
    /// �Ƿ��ƶ����
    /// </summary>
    public bool TimerFinsih = false;
    /// <summary>
    /// �ƶ���ɻص�
    /// </summary>
    protected Action mTimerFinishCalllBack;
    /// <summary>
    /// �����ж��ص�
    /// </summary>
    protected Action mUpdateTiemrCallBack;
    /// <summary>
    /// �߼�֡����
    /// </summary>
    public abstract void OnLogicFrameUpdate();
    /// <summary>
    /// �ж����
    /// </summary>
    public abstract void OnTimerFinish();
}
