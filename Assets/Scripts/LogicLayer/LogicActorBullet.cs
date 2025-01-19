using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LogicActor 
{
    /// <summary>
    /// 
    /// </summary>
    private List<SKillBulletLogic> mBulletLogicList = new List<SKillBulletLogic>();

    /// <summary>
    /// �����ӵ��߼�֡
    /// </summary>
    public void OnLogicFramUpdateBullet()
    {
        //����Ƿ���ʧЧ�ӵ����������Ƴ�
        for (int i = mBulletLogicList.Count-1; i >=0; i--)
        {
            if (mBulletLogicList[i].isFailure)
            {
                RemoveBullet(mBulletLogicList[i]);
            }
        }

        //�����ӵ��߼�֡
        foreach (var item in mBulletLogicList)
        {
            item.OnLogicFrameUpdate();
        }
    }
    //����ӵ��߼���
    public void AddBullet(SKillBulletLogic bullet)
    {
        mBulletLogicList.Add(bullet);
    }
    //�Ƴ��ӵ��߼���
    public void RemoveBullet(SKillBulletLogic bullet)
    {

        mBulletLogicList.Remove(bullet);
    }
}
