using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HideMonoScript]
[System.Serializable]
public class SkillConfig 
{
    [HideInInspector]
    public bool showStockPileData = false;//是否显示技能蓄力阶段数据

    [HideInInspector]
    public bool showSkillGuide = false;//是否显示技能引导


    [LabelText("技能图标"),LabelWidth(0.1f),PreviewField(70,ObjectFieldAlignment.Left),SuffixLabel("技能图标")]
    public Sprite skillIcon;//技能图标
    [LabelText("技能id")]
    public int skillid;//技能id
    [LabelText("技能名称")]
    public string skillName;//技能名称
    [LabelText("技能消耗魔法值")]
    public int needMgicValue;//技能消耗魔法值
    [LabelText("技能释放前摇时间ms")]
    public int skillShakeBeforeTimeMs;//技能释放前摇时间
    [LabelText("技能攻击持续时间ms")]
    public int skillAttackDurationMs;//技能攻击持续时间
    [LabelText("技能释放后摇时间ms")]
    public int skillShakeAfterMs;//技能释放后摇时间
    [LabelText("技能CD时间ms")]
    public int skillCDTimeMs;//技能CD时间
    [LabelText("技能类型"),OnValueChanged("OnSkillTypeChange")]
    public SkillType skillType;
    
    [LabelText("蓄力阶段配置（若第一阶段触发时间不为0，则空挡时间为动画表现时间）"),ShowIf("showStockPileData")]
    public List<StockPileStageData> stockPileStageData;

    [LabelText("技能引导特效"),ShowIf("showSkillGuide")]
    public GameObject skillGuideObj;//技能引导特效
    [LabelText("技能引导范围"), ShowIf("showSkillGuide")]
    public float skillGuideRange;//技能引导范围

    [LabelText("组合技能id（衔接下一个技能id）"), Tooltip("比如：技能A 由技能C B D 组成")]
    public int ComobinationSkillid;

    //技能渲染相关
    [LabelText("技能命中特效"),TitleGroup("技能渲染","所有英雄渲染数据会在开始释放技能时触发")]
    public GameObject skillHitEffect; //技能命中特效
    [LabelText("命中特效显示时间"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发")]
    public int hitEffectSurivalTimeMs = 100;//命中特效显示时间
    [LabelText("技能命中音效"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发")]
    public AudioClip skillHitAudio; //技能命中音效
    [LabelText("是否显示技能立绘"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发")]
    public bool showSkillPortrait; //是否显示技能立绘
    [LabelText("技能立绘"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发")]
    [ShowIf("showSkillPortrait")]
    public GameObject skillPortrait; //技能立绘
    [LabelText("技能描述")]
    public string skillDes; //技能描述


    public void OnSkillTypeChange(SkillType skillType)
    {
        showStockPileData = skillType == SkillType.StockPlie;
        showSkillGuide = skillType == SkillType.PosGuide;
    }

}


public enum SkillType
{
    [LabelText("无配置（瞬发技能）")]None,//无（瞬发）
    [LabelText("吟唱型技能")] Chnat,//吟唱型
    [LabelText("弹道型技能")] Ballistic,//弹道型
    [LabelText("蓄力型技能")] StockPlie,//蓄力型
    [LabelText("位置引导型技能")] PosGuide,//位置引导型
}

/// <summary>
/// 技能蓄力阶段数据
/// </summary>
[System.Serializable]
public class StockPileStageData
{
    [LabelText("蓄力阶段")]
    public int stage; //蓄力阶段
    [LabelText("当前蓄力阶段触发的技能id")]
    public int skillid; //当前蓄力阶段触发的技能id
    [LabelText("当前阶段开始时间")]

    public int startTimeMs; //当前阶段开始时间
    [LabelText("当前阶段结束时间")]
    public int endTimeMs; //当前阶段结束时间
}


