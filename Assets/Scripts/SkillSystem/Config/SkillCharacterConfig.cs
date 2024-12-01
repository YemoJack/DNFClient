using Sirenix.OdinInspector;
using Sirenix.Reflection.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[HideMonoScript]
[System.Serializable]
public class SkillCharacterConfig 
{

    [AssetList]
    [LabelText("角色模型")]
    [PreviewField(70,ObjectFieldAlignment.Center)]
    public GameObject skillCharacter;

    [LabelText("技能动画")]
    [TitleGroup("技能渲染","所有英雄渲染数据会在技能开始释放时触发")]
    public AnimationClip SkillAnim;

    [BoxGroup("动画数据")]
    [ProgressBar(0,100,r:0,g:255,b:0,Height = 30)]
    [OnValueChanged("OnAnimProgressValueChange")]
    [HideLabel]
    public short animProgress = 0;

    [BoxGroup("动画数据")]
    [LabelText("是否循环动画")]
    public bool isLoopAnim = false;
    [BoxGroup("动画数据")]
    [LabelText("动画循环次数")]
    [ShowIf("isLoopAnim")]
    public int animLoopCount;
    [BoxGroup("动画数据")]
    [LabelText("逻辑帧数")]
    public int logicFrame = 0;
    [BoxGroup("动画数据")]
    [LabelText("动画长度")]
    public float animLength = 0;
    [BoxGroup("动画数据")]
    [LabelText("技能持续时间 （毫秒ms）")]
    public float skillDurationMS = 0;


    private GameObject mTempCharacter;
    private bool mIsPlayAnim = false;//是否播放动画，用来控制暂停动画
    private double mLastRunTime = 0;//上次运行的时间
    private Animation mAnimation;

    [GUIColor(0.4f,0.8f,1)]
    [ButtonGroup("按钮数组")]
    [Button("播放",ButtonSizes.Large)]
    public void Play()
    {
        if(skillCharacter!=null)
        {
            //先从场景上查找技能对象，如果查找不到，就克隆一个
            string charactorName = skillCharacter.name;
            mTempCharacter = GameObject.Find(charactorName);
            if (mTempCharacter == null)
            {
                mTempCharacter = GameObject.Instantiate(skillCharacter);
            }
            //判断模型上是否有该动画，如果没有则进行添加
            mAnimation = mTempCharacter.GetComponent<Animation>();
            if (!mAnimation.GetClip(SkillAnim.name))
            {
                mAnimation.AddClip(SkillAnim, SkillAnim.name);
            }
            mAnimation.clip = SkillAnim;
            //计算动画长度
            animLength = isLoopAnim ? SkillAnim.length*animLoopCount : SkillAnim.length;
            //计算逻辑帧长度（个数）
            logicFrame = (int)(isLoopAnim?SkillAnim.length/0.066f*animLoopCount:SkillAnim.length/0.066f);

            skillDurationMS = isLoopAnim ?(SkillAnim.length*animLoopCount)*1000: SkillAnim.length*1000;

            mLastRunTime = 0;
            //开始播放动画
            mIsPlayAnim = true;
            SkillComplierWindow window = SkillComplierWindow.GetWindow();
            window?.StartPlaySkill();

        }




    }


    [ButtonGroup("按钮数组")]
    [Button("暂停", ButtonSizes.Large)]
    public void Pause()
    {
        mIsPlayAnim=false;
        SkillComplierWindow window = SkillComplierWindow.GetWindow();
        window?.SkillPause();
    }

    [GUIColor(0f, 1f, 0)]
    [ButtonGroup("按钮数组")]
    [Button("保存配置", ButtonSizes.Large)]
    public void SaveAsset()
    {

    }


    public void OnUpdate(Action updateCallback)
    {
        if(mIsPlayAnim)
        {
            if(mLastRunTime == 0)
            {
                mLastRunTime = EditorApplication.timeSinceStartup;
            }

            double curRunTime = EditorApplication.timeSinceStartup - mLastRunTime;
            //计算动画播放进度
            float curAnimNormalizationValue = (float)curRunTime / animLength;
            animProgress = (short)Mathf.Clamp(curAnimNormalizationValue * 100, 0, 100);
            //计算逻辑帧
            logicFrame = (int)(curRunTime / LogicFrameConfig.LogicFrameInterval);
            //采样动画，进行动画播放
            mAnimation.clip.SampleAnimation(mTempCharacter, (float)curRunTime);

            if(animProgress == 100)
            {
                //播放结束
                PlaySkillEnd();
            }
            updateCallback?.Invoke();
        }
    }

    /// <summary>
    /// 动画进度值改变监听
    /// </summary>
    /// <param name="value"></param>
    public void OnAnimProgressValueChange(float value)
    {
        //先从场景上查找技能对象，如果查找不到，就克隆一个
        string charactorName = skillCharacter.name;
        mTempCharacter = GameObject.Find(charactorName);
        if (mTempCharacter == null)
        {
            mTempCharacter = GameObject.Instantiate(skillCharacter);
        }
        //判断模型上是否有该动画，如果没有则进行添加
        mAnimation = mTempCharacter.GetComponent<Animation>();
        float progressValue = (value / 100f) * SkillAnim.length;
        logicFrame = (int)(progressValue / LogicFrameConfig.LogicFrameInterval);

        mAnimation.clip.SampleAnimation(mTempCharacter, progressValue);
    }



    public void PlaySkillEnd()
    {
        mIsPlayAnim = false;

        SkillComplierWindow window = SkillComplierWindow.GetWindow();
        window?.PlaySkillEnd();
    }


}
