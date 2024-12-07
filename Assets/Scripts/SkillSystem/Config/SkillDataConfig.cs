using Sirenix.OdinInspector;

using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


[CreateAssetMenu(fileName = "SkillConfig",menuName = "SkillConfig",order = 0)]
public class SkillDataConfig : ScriptableObject
{

    /// <summary>
    /// 角色模型配置
    /// </summary>
    public SkillCharacterConfig character;
    /// <summary>
    /// 技能配置
    /// </summary>
    public SkillConfig skillCfg;
    /// <summary>
    /// 伤害配置
    /// </summary>
    public List<SkillDamageConfig> damageCfgList;
    /// <summary>
    /// 特效配置
    /// </summary>
    public List<SkillEffectConfig> effectCfgList;


#if UNITY_EDITOR

    /// <summary>
    /// 保存技能配置
    /// </summary>
    /// <param name="characterCfg"></param>
    /// <param name="skillCfg"></param>
    /// <param name="damageCfgList"></param>
    /// <param name="effectCfgList"></param>
    public static void SaveSkillData(SkillCharacterConfig characterCfg,SkillConfig skillCfg,List<SkillDamageConfig> damageCfgList,List<SkillEffectConfig> effectCfgList)
    {
        //通过代码创建SkillDataConfig的实例，并对字段进行赋值存储
        SkillDataConfig skillDataCfg = ScriptableObject.CreateInstance<SkillDataConfig>();
        skillDataCfg.character = characterCfg;
        skillDataCfg.skillCfg = skillCfg;
        skillDataCfg.damageCfgList = damageCfgList;
        skillDataCfg.effectCfgList = effectCfgList;
        //把当前实例存储为.asset文件 当作技能配置
        string assetPath = "Assets/GameData/Game/SkillSystem/SkillData/" + skillCfg.skillid + ".asset";


        //如果资源已经存在，先删除
        AssetDatabase.DeleteAsset(assetPath);
        AssetDatabase.CreateAsset(skillDataCfg,assetPath);
    }

    [Button("配置技能",ButtonSizes.Large),GUIColor("green")]
    public void ShowSkillWindowButtonClick()
    {
        SkillComplierWindow window = SkillComplierWindow.ShowWindow();
        window.LoadSkillData(this);
    }

#endif

}
