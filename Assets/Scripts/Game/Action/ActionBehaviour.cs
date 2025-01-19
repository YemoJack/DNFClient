using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBehaviour  
{
    /// <summary>
    /// �Ƿ��ƶ����
    /// </summary>
    public bool actionFinsih = false;
    /// <summary>
    /// �ƶ���ɻص�
    /// </summary>
    protected Action mActionFinishCalllBack;
    /// <summary>
    /// �����ж��ص�
    /// </summary>
    protected Action mUpdateActionCallBack;
    /// <summary>
    /// �߼�֡����
    /// </summary>
    public abstract void OnLogicFrameUpdate();
    /// <summary>
    /// �ж����
    /// </summary>
    public abstract void OnActionFinish();
}
