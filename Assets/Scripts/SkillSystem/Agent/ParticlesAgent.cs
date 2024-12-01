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
        mParticleArr = trans.GetComponentsInChildren<ParticleSystem>();
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
        //获取当前运行时间
        double curRunTime = EditorApplication.timeSinceStartup - mLastRunTime;
        //模拟粒子特效播放进度
        if(mParticleArr != null)
        {
            foreach(var particle in mParticleArr)
            {
                if(particle != null)
                {
                    //停止所有粒子动效的播放
                    particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    //关闭由随机种子播放的粒子特效
                    particle.useAutoRandomSeed = false;
                    //模拟粒子特效的播放
                    particle.Simulate((float)curRunTime);
                }
            }
        }



    }




#endif

}
