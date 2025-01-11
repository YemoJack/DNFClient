using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;

/// <summary>
///  buff系统  管理Buff 释放移除更新
/// </summary>
public class BuffSystem : Singleton<BuffSystem>
{

    /// <summary>
    /// buff列表
    /// </summary>
    private List<Buff> mBuffList = new List<Buff>();

    public void OnCreate()
    {

    }

    /// <summary>
    /// 附加一个buff 
    /// </summary>
    /// <param name="buffid">buff的id</param>
    /// <param name="releaser">buff的释放者</param>
    /// <param name="attachTarget">buff附加的目标</param>
    /// <param name="skill">buff依附的技能</param>
    /// <param name="paramsObjs">buff所需要的参数</param>
    /// <returns></returns>
    public Buff AttachBuff(int buffid, LogicActor releaser, LogicActor attachTarget, Skill skill, object[] paramsObjs = null)
    {
        if (buffid <= 0)
        {
            Debug.LogError("buffid 不合法 必须为非负值 当前附加buff为无效buff");
            return null;
        }
        Buff buff = new Buff(buffid, releaser, attachTarget, skill, paramsObjs);
        buff.OnCreate();
        mBuffList.Add(buff);
        return buff;
    }


    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdate()
    {
        for (int i = mBuffList.Count-1; i >= 0 ; i--)
        {
            mBuffList[i].OnLogicFrameUpdate();
        }
    }

    /// <summary>
    /// 移除指定Buff
    /// </summary>
    /// <param name="buff"></param>
    public void RemoveBuff(Buff buff)
    {
        if (mBuffList.Contains(buff))
        {
            mBuffList.Remove(buff);
        }
    }



    public void OnDestroy()
    {
        for (int i = mBuffList.Count - 1; i >= 0; i--)
        {
            mBuffList[i].OnDestroy();
        }
    }

}
