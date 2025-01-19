using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;

/// <summary>
/// �������е�buff �ͷš��Ƴ� �������߼� 
/// Buff ȫ�����ͣ��������ͣ���ͼ���ͣ���ɫ������GM 
/// </summary>
public class BuffSystem : Singleton<BuffSystem>
{
    /// <summary>
    /// ����buff�б�
    /// </summary>
    private List<Buff> mBuffList = new List<Buff>();


    public void OnCreate()
    {

    }
    /// <summary>
    /// ����һ��buff
    /// </summary>
    /// <param name="buffid">id</param>
    /// <param name="releaser">Buffʩ����</param>
    /// <param name="attachTarget">Buff����Ŀ��</param>
    /// <param name="skill">buff��Դ</param>
    /// <param name="paramsObjs">buff�������</param>
    /// <returns></returns>
    public Buff AttachBuff(int buffid, LogicActor releaser, LogicActor attachTarget, Skill skill, object[] paramsObjs=null)
    {
        if (buffid==0)
        {
            Debug.LogError("Buff id ����Ϊ0����ǰ����BuffΪ��Чbuff��");
            return null;
        }
        Buff buff = new Buff(buffid,releaser,attachTarget,skill,paramsObjs);
        buff.OnCreate();
        mBuffList.Add(buff);
        return buff;
    }
    /// <summary>
    /// �߼�֡���½ӿ�
    /// </summary>
    public void OnLogicFrameUpdate()
    {
        for (int i = mBuffList.Count-1; i >=0; i--)
        {
            mBuffList[i].OnLogicFrameUpdate();
        }
    }
    /// <summary>
    /// �Ƴ�ָ����buff
    /// </summary>
    /// <param name="buff"></param>
    public void RemoveBuff(Buff buff)
    {
        if (mBuffList.Contains(buff))
        {
            mBuffList.Remove(buff);
        }
    }
    public void OnDestory()
    {
        for (int i = mBuffList.Count - 1; i >= 0; i--)
        {
            mBuffList[i].OnDestroy();
        }
    }

}
