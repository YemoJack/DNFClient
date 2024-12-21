using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour
{

    public Text cdText;

    public Image iconInmage;

    public Image cdMaskImage;

    public SKillItem_JoyStick skillJoyStick;

    private Skill mSkillData;
    private LogicActor mSkillCreater;

    public void SetItemSkillData(Skill skillData, LogicActor skillCreater)
    {
        mSkillData = skillData;
        mSkillCreater = skillCreater;
        skillJoyStick.InitSkillData(GetSkillGuideType(skillData.SkillConfig.skillType),skillData.skillid,skillData.SkillConfig.skillGuideRange);
        skillJoyStick.OnReleaseSkill += OnTriggerSkill;
        skillJoyStick.OnSkillGuide += OnUpdateSkillGuide;
        iconInmage.sprite = skillData.SkillConfig.skillIcon;
        cdMaskImage.gameObject.SetActive(false);
        cdText.gameObject.SetActive(false);
    }



    void Update()
    {
        
    }

    /// <summary>
    /// 触发对应的技能 （释放摇杆时触发）
    /// </summary>
    /// <param name="sKillGuide">技能引导类型</param>
    /// <param name="skillPos">技能触发位置</param>
    /// <param name="skillId">技能触发id</param>
    public void OnTriggerSkill(SKillGuideType sKillGuide, Vector3 skillPos,int skillId)
    {
        if(sKillGuide == SKillGuideType.Click)
        {
            mSkillCreater.ReleaseSkill(skillId);
        }
        else if(sKillGuide == SKillGuideType.LongPress)
        {
            //触发蓄力技能 TODO
        }
        else if(sKillGuide == SKillGuideType.Position)
        {
            //指定位置技能 TODO
        }
    }


    /// <summary>
    /// 更新技能引导回调
    /// </summary>
    /// <param name="sKillGuide">技能引导类型</param>
    /// <param name="isCancel">是否取消</param>
    /// <param name="skillPos">技能释放位置</param>
    /// <param name="skillId">技能id</param>
    /// <param name="skillDirDis">技能半径</param>
    public void OnUpdateSkillGuide(SKillGuideType sKillGuide,bool isCancel,Vector3 skillPos,int skillId,float skillDirDis)
    {

    }


    /// <summary>
    /// 根据技能类型获取技能引导类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public SKillGuideType GetSkillGuideType(SkillType type)
    {
        SKillGuideType sKillGuide = SKillGuideType.Click;
        if(type == SkillType.StockPlie)
        {
            sKillGuide = SKillGuideType.LongPress;
        }
        else if(type == SkillType.Chnat || type == SkillType.Ballistic || type == SkillType.None)
        {
            sKillGuide = SKillGuideType.Click;
        }
        else if(type == SkillType.PosGuide)
        {
            sKillGuide = SKillGuideType.Position;
        }
       
        return sKillGuide;
    }


}
