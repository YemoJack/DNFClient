using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;

public partial class Skill
{
    //��ǰ�����ӵ��ۼ�ʱ���б�
    private List<int> mCurCreateBulletAccTimeList = new List<int>();
    /// <summary>
    /// ������������������
    /// </summary>
    private LogicRandom mLogicRandom;
    /// <summary>
    /// ��ʼ���ӵ��������
    /// </summary>
    public void OnBulletInit()
    {
        mLogicRandom = new LogicRandom(10);
        if (mSkillData.bulletCfgList != null && mSkillData.bulletCfgList.Count > 0)
        {
            for (int i = 0; i < mSkillData.bulletCfgList.Count; i++)
            {
                mCurCreateBulletAccTimeList.Add(0);
            }
        }
    }
    public void OnLogicFrameUpdateBullet()
    {
        if (mSkillData.bulletCfgList != null && mSkillData.bulletCfgList.Count > 0)
        {

            for (int i = 0; i < mSkillData.bulletCfgList.Count; i++)
            {
                mCurCreateBulletAccTimeList[i]+= LogicFrameConfig.LogicFrameIntervalms; 
                SkillBulletConfig item=  mSkillData.bulletCfgList[i];
                if (item.triggerFrame == mCurLogicFrame)
                {
                    //��Ҫ�����ӵ�
                    CreateBullet(item);
                }
                //�ж��ӵ��Ƿ���ѭ������
                if (item.isLoopCreate)
                {
                    //��ǿ����³����
                    if (item.loopIntervalMs==0)
                    {
                        Debug.LogError("item.loopIntervalMs == 0,�������ӵ�ѭ������");
                        continue;
                    }
                    //��ǰ�����ӵ��ۼ�ʱ�䳬���˴����ӵ��ļ�����ʹ����ӵ�
                    while (mCurCreateBulletAccTimeList[i] >= item.loopIntervalMs)
                    {
                        CreateBullet(item);
                        mCurCreateBulletAccTimeList[i] -= item.loopIntervalMs;
                    }
                }
            }
        }
    }
  
    /// <summary>
    /// �����ӵ�
    /// </summary>
    /// <param name="config">�����ӵ�����</param>
    public void CreateBullet(SkillBulletConfig config)
    {
        //�򵥴��������ٶԿ�ܵ�������Ŀ����Ϊ���ô��ѧϰ�������Ӽ򵥣���ֲ�������ӷ��㡣
        //����ͨ����Դ��ܽ��д�������ͨ����Դ����ؽ��й���
        //GameObject bulletObj = GameObject.Instantiate(config.bulletPrefab);
        GameObject bulletObj = ZMAssetsFrame.Instantiate(config.bulletPrefabPath, null);
        //������Ⱦ��
        SkillBulletRender bulletRender= bulletObj.GetComponent<SkillBulletRender>();
        if (bulletRender==null)
        {
            bulletRender= bulletObj.AddComponent<SkillBulletRender>();
        }

        FixIntVector3 rangePos = FixIntVector3.zero;
        if (config.isLoopCreate)
        {
            //���xyz������
            FixInt x = mLogicRandom.Range(config.minrandomRangeVect3.x, config.maxRandomRangeVect3.x);
            FixInt y = mLogicRandom.Range(config.minrandomRangeVect3.y, config.maxRandomRangeVect3.y);
            FixInt z = mLogicRandom.Range(config.minrandomRangeVect3.z, config.maxRandomRangeVect3.z);
            rangePos = new FixIntVector3(x, y, z);
        }
        //�����߼���
        SKillBulletLogic bulletLogic = new SKillBulletLogic(this,mSkillCreater, bulletRender, config, rangePos);
        bulletRender.SetRenderData(bulletLogic,config);
        mSkillCreater.AddBullet(bulletLogic);
    }

    public void OnBulletRelease()
    {
        mCurCreateBulletAccTimeList.Clear();
        mLogicRandom=null;
    }
}
