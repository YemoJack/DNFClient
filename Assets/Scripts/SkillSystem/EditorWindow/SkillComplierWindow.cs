using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillComplierWindow : OdinEditorWindow
{
    [TabGroup("Skill","模型动画数据",SdfIconType.PersonFill,TextColor ="orange")]
    public SkillCharacterConfig character = new SkillCharacterConfig();
    [TabGroup("SkillComplier","Skill",SdfIconType.Robot,TextColor ="lightmagenta")]
    public SkillConfig skill = new SkillConfig();

    [TabGroup("SkillComplier", "Damage", SdfIconType.At, TextColor = "lightmagenta")]
    public List<SkillDamageConfig> damageList = new List<SkillDamageConfig>();

    [TabGroup("SkillComplier", "Effect", SdfIconType.OpticalAudio, TextColor = "blue")]
    public List<SkillEffectConfig> effectList = new List<SkillEffectConfig>();

#if UNITY_EDITOR
    //技能是否开始播放
    private bool isStartPlaySkill = false;


    [MenuItem("Skill/技能编辑器")]
    public static SkillComplierWindow ShowWindow()
    {
        return GetWindowWithRect<SkillComplierWindow>( new Rect(0, 0, 1000, 600));
    }

    /// <summary>
    /// 获取Editor模式下的角色位置
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetCharacterPos()
    {
        SkillComplierWindow window = GetWindow<SkillComplierWindow>();
        if(window.character.skillCharacter!= null)
        {
            return window.character.skillCharacter.transform.position;
        }
        return Vector3.zero;
    }

    public static SkillComplierWindow GetWindow()
    {
        return GetWindow<SkillComplierWindow>();
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        EditorApplication.update += OnEditorUpdate;
    }


    protected override void OnDisable()
    {
        base.OnDisable();
        EditorApplication.update -= OnEditorUpdate;
    }

    /// <summary>
    /// 开始播放技能
    /// </summary>
    public void StartPlaySkill()
    {
        foreach(var item in effectList)
        {
            item.StartPlaySkill();
        }

        foreach(var item in damageList)
        {
            item.PlaySkillStart();
        }
        mAccLogicRunTime = 0;
        mNextLogicFrameTime = 0;
        mLastUpdateTime = 0;

        isStartPlaySkill = true;
    }

    /// <summary>
    /// 技能暂停
    /// </summary>
    public void SkillPause()
    {
        foreach(var item in effectList)
        {
            item.SkillPause();
        }
    }


    /// <summary>
    /// 播放技能结束
    /// </summary>
    public void PlaySkillEnd()
    {
        foreach (var item in effectList)
        {
            item.PlaySkillEnd();
        }
        foreach (var item in damageList)
        {
            item.PlaySkillEnd();
        }

        mAccLogicRunTime = 0;
        mNextLogicFrameTime = 0;
        mLastUpdateTime = 0;

        isStartPlaySkill = false;
    }


    public void OnEditorUpdate()
    {
        try
        {
            character.OnUpdate(() =>
            {
                //刷新窗口
                Focus();
            });

            if (isStartPlaySkill)
            {
                OnLogicUpdate();
            }

        }
        catch(System.Exception e)
        {

        }
    }


    private float mAccLogicRunTime;     //累计逻辑帧时间
    private float mNextLogicFrameTime;  //下一个逻辑帧的时间
    private float mDeltaTime;           //动画缓动时间 当前帧的增量时间
    private double mLastUpdateTime;     //上次更新的时间

    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public void OnLogicUpdate()
    {
        //模拟帧同步更新
        if(mLastUpdateTime == 0)
        {
            mLastUpdateTime = EditorApplication.timeSinceStartup;
        }

        //计算逻辑帧累计运行时间
        mAccLogicRunTime = (float)(EditorApplication.timeSinceStartup - mLastUpdateTime);
        while(mAccLogicRunTime > mNextLogicFrameTime)
        {
            OnLogicFrameUpdate();
            //下一个逻辑帧的时间
            mNextLogicFrameTime += LogicFrameConfig.LogicFrameInterval;
        }
    }

    public void OnLogicFrameUpdate()
    {
        foreach(var effect in effectList)
        {
            effect.OnLogicFrameUpdate();
        }

        foreach(var damage in damageList)
        {
            damage.OnLogicFrameUpdate();
        }
    }


#endif
}
