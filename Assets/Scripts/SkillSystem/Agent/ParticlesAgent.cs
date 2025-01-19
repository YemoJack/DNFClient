using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ParticlesAgent : MonoBehaviour
{
#if UNITY_EDITOR

    private ParticleSystem[] mParticleArr;
    private double mLastRunTime;
    public void InitPlayAnim(Transform trans)
    {
        mParticleArr= trans.GetComponentsInChildren<ParticleSystem>();
        EditorApplication.update += OnUpdate;
    }

    public void OnDestroy()
    {
        EditorApplication.update -= OnUpdate;
    }
    public void OnUpdate()
    {
        if (mLastRunTime == 0)
        {
            mLastRunTime = EditorApplication.timeSinceStartup;
        }
        //��ȡ��ǰ���е�ʱ��
        double curRunTime = EditorApplication.timeSinceStartup - mLastRunTime;

        if (mParticleArr!=null)
        {
            foreach (var item in mParticleArr)
            {
                if (item!=null)
                {
                    //ֹͣ�������Ӷ�Ч�Ĳ���
                    item.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    //�ر���������Ӳ��ŵ�������Ч
                    item.useAutoRandomSeed = false;
                    //ģ�����Ӷ�Ч�Ĳ���
                    item.Simulate((float)curRunTime);
                }
            }
        }
    }
#endif
}
