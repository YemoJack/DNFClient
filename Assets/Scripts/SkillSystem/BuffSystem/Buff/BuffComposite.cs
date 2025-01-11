using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffComposite 
{


    protected Buff mBuff;

    public BuffComposite(Buff buff)
    {
        mBuff = buff;
    }
    
    /// <summary>
    /// buff延迟触发
    /// </summary>
    public abstract void BuffDelay();
    /// <summary>
    /// buff开始
    /// </summary>
    public abstract void BuffStart();
    /// <summary>
    /// Buff触发 效果
    /// </summary>
    public abstract void BuffTrigger();

    /// <summary>
    /// buff结束
    /// </summary>
    public abstract void BuffEnd();


}
