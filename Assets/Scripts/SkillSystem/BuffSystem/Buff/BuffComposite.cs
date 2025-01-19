using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffComposite 
{

    protected Buff mBuff;

    public BuffComposite(Buff buff)
    {
        this.mBuff = buff;
    }

    /// <summary>
    /// buff�ӳٴ����ӿ�
    /// </summary>
    public abstract void BuffDelay();
    /// <summary>
    /// Buff��ʼ����
    /// </summary>
    public abstract void BuffStart();
    /// <summary>
    /// buff�߼�����������ִ����ѣ�߼��������޸��߼�
    /// </summary>
    public abstract void BuffTrigger();
    /// <summary>
    /// buffִ�����
    /// </summary>
    public abstract void BuffEnd();
}
