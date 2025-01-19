using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillComplierWindow : OdinEditorWindow
{
    [TabGroup("Skill","Chararcter", SdfIconType.PersonFill, TextColor ="orange")]
    public SkillCharacterConfig character = new SkillCharacterConfig();
    [TabGroup("SKillComplier","Skill",SdfIconType.Robot,TextColor ="lightmagenta")]
    public SkillConfig skill = new SkillConfig();

    [TabGroup("SKillComplier", "Buff", SdfIconType.Magic, TextColor = "red")]
    public List<SkillBuffConfig> buffList = new List<SkillBuffConfig>();

    [TabGroup("SKillComplier", "Damage", SdfIconType.At, TextColor = "lightmagenta")]
    public List<SkillDamageConfig> damageList = new List<SkillDamageConfig>();

    [TabGroup("SKillComplier", "Effect", SdfIconType.OpticalAudio, TextColor = "blue")]
    public List<SkillEffectConfig> effectList = new List<SkillEffectConfig>();

    [TabGroup("SKillComplier", "Audio", SdfIconType.OpticalAudio, TextColor = "blue")]
    public List<SkillAudioConfig> audioList = new List<SkillAudioConfig>();

    [TabGroup("SKillComplier", "Bullet", SdfIconType.OpticalAudio, TextColor = "cyan")]
    public List<SkillBulletConfig> bulletList = new List<SkillBulletConfig>();

    [TabGroup("SKillComplier", "Action", SdfIconType.OpticalAudio, TextColor = "cyan")]
    public List<SkillActionConfig> actionList = new List<SkillActionConfig>();
#if UNITY_EDITOR
    //�Ƿ�ʼ���ż���
    private bool isStartPlaySkill = false;
    [MenuItem("SKill/���ܱ�����")]
    public static SkillComplierWindow ShowWindow()
    {
       return GetWindowWithRect<SkillComplierWindow>(new Rect(0,0,1000,600));
    }
    public void SaveSKillData()
    {
        SkillDataConfig.SaveSkillData(character, skill, damageList, effectList, audioList, actionList, bulletList,buffList);
        Close();
    }
    /// <summary>
    /// ���ؼ�������
    /// </summary>
    /// <param name="skillData"></param>
    public void LoadSkillData(SkillDataConfig skillData)
    {
        this.character = skillData.character;
        this.skill = skillData.skillCfg;
        this.damageList = skillData.damageCfgList;
        this.effectList = skillData.effectCfgList;
        this.audioList = skillData.audioCfgList;
        this.actionList = skillData.actionCfgList;
        this.bulletList = skillData.bulletCfgList;
        this.buffList = skillData.buffCfgList;
    }
    public static SkillComplierWindow GetWindow()
    {
        
        return GetWindow<SkillComplierWindow>();
    }
    /// <summary>
    /// ��ȡEditorģʽ�½�ɫλ��
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetCharaterPos()
    {
        if (!HasOpenInstances<SkillComplierWindow>())
        {
            return Vector3.zero;
        }
        SkillComplierWindow window = GetWindow<SkillComplierWindow>();
  
        if (window.character.skillChararcter!=null)
        {
            return window.character.skillChararcter.transform.position;
        }
        return Vector3.zero;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        foreach (var item in damageList)
        {
            item.OnInit();
        }
        EditorApplication.update += OnEditorUpdate;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        foreach (var item in damageList)
        {
            item.OnRelease();
        }
        EditorApplication.update -= OnEditorUpdate;
    }
    /// <summary>
    /// ��ʼ���ż���
    /// </summary>
    public void StartPlaySkill()
    {
        foreach (var item in effectList)
        {
            item.StartPlaySkill();
        }
        foreach (var item in damageList)
        {
            item.PlaySkillStart();
        }
        mAccLogicRuntime = 0;
        mNextLogicFrameTime = 0;
        mLastUpdateTime = 0;
        isStartPlaySkill = true;
    }
    /// <summary>
    /// ������ͣ
    /// </summary>
    public void SkillPause()
    {
        foreach (var item in effectList)
        {
            item.PlaySkillEnd();
        }
        foreach (var item in damageList)
        {
            item.PlaySkilEnd();
        }
    }
    /// <summary>
    /// ���ż��ܽ���
    /// </summary>
    public void PlaySkilEnd()
    {
        foreach (var item in effectList)
        {
            item.PlaySkillEnd();
        }
        foreach (var item in damageList)
        {
            item.PlaySkilEnd();
        }
        isStartPlaySkill = false;
        mAccLogicRuntime = 0;
        mNextLogicFrameTime = 0;
        mLastUpdateTime = 0;
    }

    public void OnEditorUpdate()
    {
        try
        {
            character.OnUpdate(()=> {
                //ˢ�µ�ǰ����
                Focus();
            });
            if (isStartPlaySkill)
            {
                OnLogicUpdate();
            }
 
        }
        catch (System.Exception)
        {

        }
    }

    private float mAccLogicRuntime;//�߼�֡�ۼ�����ʱ��
    private float mNextLogicFrameTime;//��һ���߼�֡��ʱ��
    private float mDeltaTime;//��������ʱ�� ��ǰ֡������ʱ��
    private double mLastUpdateTime;//�ϴθ��µ�ʱ��
    /// <summary>
    /// �߼�Update
    /// </summary>
    public void OnLogicUpdate()
    {
        //ģ��֡ͬ������ ��0.066��ļ�����и���
        if (mLastUpdateTime==0)
        {
            mLastUpdateTime = EditorApplication.timeSinceStartup;
        }
        //�����߼�֡�ۼ�����ʱ��
        mAccLogicRuntime =(float)(EditorApplication.timeSinceStartup - mLastUpdateTime);
        while (mAccLogicRuntime>mNextLogicFrameTime)
        {
            OnLogicFrameUpdate();
            //��һ���߼�֡��ʱ��
            mNextLogicFrameTime += LogicFrameConfig.LogicFrameInterval;
        }
    }
    public void OnLogicFrameUpdate()
    {
        foreach (var item in effectList)
        {
            item.OnLogicFrameUpdate();
        }
        foreach (var item in damageList)
        {
            item.OnLogicFrameUpdate();
        }
    }
#endif
}
