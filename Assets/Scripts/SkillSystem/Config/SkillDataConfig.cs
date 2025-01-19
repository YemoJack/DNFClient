using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
# if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
[CreateAssetMenu(fileName ="SkillConfig",menuName ="SkillConfig", order =0)]
public class SkillDataConfig : ScriptableObject
{
    //��ɫ��������
    public SkillCharacterConfig character;
    //���ܻ�����������
    public SkillConfig skillCfg;
    //�����˺������б�
    public List<SkillDamageConfig> damageCfgList;
    //������Ч�����б�
    public List<SkillEffectConfig> effectCfgList;
    //������Ч�����б�
    public List<SkillAudioConfig> audioCfgList;
    //������Ч�����б�
    public List<SkillBulletConfig> bulletCfgList;
    //�ж������б�
    public List<SkillActionConfig> actionCfgList;
    //buff�����б�
    public List<SkillBuffConfig> buffCfgList;

    
#if UNITY_EDITOR
    public static void SaveSkillData(SkillCharacterConfig characterCfg,SkillConfig skillCfg,List<SkillDamageConfig> damageCfgList, List<SkillEffectConfig> effectCfgList
        , List<SkillAudioConfig> audioCfgList, List<SkillActionConfig> actionCfgList, List<SkillBulletConfig> bulletCfgList, List<SkillBuffConfig> buffCfgList)
    {
        //ͨ�����봴��SkillDataConfig��ʵ���������ֶν��и�ֵ����
        SkillDataConfig skillDataCfg= ScriptableObject.CreateInstance<SkillDataConfig>();
        skillDataCfg.character = characterCfg;
        skillDataCfg.skillCfg = skillCfg;
        skillDataCfg.damageCfgList = damageCfgList;
        skillDataCfg.effectCfgList = effectCfgList;
        skillDataCfg.audioCfgList = audioCfgList;
        skillDataCfg.actionCfgList = actionCfgList;
        skillDataCfg.bulletCfgList = bulletCfgList;
        skillDataCfg.buffCfgList = buffCfgList;
        //�ѵ�ǰʵ������Ϊ.asset��Դ�ļ���������������
        string assetPath = "Assets/GameData/Game/SkillSystem/SkillData/" + skillCfg.skillid + ".asset";
        //�����Դ�����Ѵ��ڣ��Ƚ���ɾ�����ڽ��д���
        AssetDatabase.DeleteAsset(assetPath);
        AssetDatabase.CreateAsset(skillDataCfg, assetPath);
    }

    [Button("���ü���",ButtonSizes.Large),GUIColor("green")]
    public void ShowSkillWindowButtonClick()
    {
        SkillComplierWindow window= SkillComplierWindow.ShowWindow();
        window.LoadSkillData(this);
    }

    public  void SaveAsset()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}

