/*---------------------------------
 *Title:UI表现层脚本自动化生成工具
 *Author:ZM 铸梦
 *Date:2024/12/8 19:23:31
 *Description:UI 表现层，该层只负责界面的交互、表现相关的更新，不允许编写任何业务逻辑代码
 *注意:以下文件是自动生成的，再次生成不会覆盖原有的代码，会在原有的代码上进行新增，可放心使用
---------------------------------*/
using UnityEngine.UI;
using UnityEngine;
using ZMUIFrameWork;
using ZMGC.Battle;
using System.Collections.Generic;

public class BattleWindow : WindowBase
{

    public BattleWindowDataComponent dataCompt;

    private HeroLogic mHeroLogic;

    /// <summary>
    /// 技能按钮节点列表
    /// </summary>
    private List<Transform> mSkillItemRootList;

    private List<SkillItem> mSkillItemList;

    #region 声明周期函数
    //调用机制与Mono Awake一致
    public override void OnAwake()
    {
        dataCompt = gameObject.GetComponent<BattleWindowDataComponent>();
        dataCompt.InitComponent(this);
        base.OnAwake();
        mSkillItemRootList = new List<Transform>();
        mSkillItemList = new List<SkillItem>();
        for(int i = 0; i < dataCompt.SkillRootTransform.childCount; i++)
        {
            mSkillItemRootList.Add(dataCompt.SkillRootTransform.GetChild(i));
        }

    }
    //物体显示时执行
    public override void OnShow()
    {
        base.OnShow();
        mHeroLogic = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>().HeroLogic;


        //获取角色技能id数组
        int[] heroSkillidArr = BattleWorld.GetExitsDataMgr<HeroDataMgr>().GetHeroSkillArr(mHeroLogic.Heroid);
        //遍历角色技能数组 生成对应的按钮
        for(int i = 0;i < heroSkillidArr.Length;i++)
        {
            GameObject skillItemObj = ZM.AssetFrameWork.ZMAssetsFrame.Instantiate(AssetPathConfig.GAME_PREFABS + "Item/SkillItem.prefab", mSkillItemRootList[i]);
            SkillItem item = skillItemObj.GetComponent<SkillItem>();
            mSkillItemList.Add(item);
            item.SetItemSkillData(mHeroLogic.GetSKill(heroSkillidArr[i]), mHeroLogic);
            item.transform.localPosition = Vector3.zero;
            item.transform.rotation = Quaternion.identity;
            item.transform.localScale = Vector3.one;
        }
    }
    //物体隐藏时执行
    public override void OnHide()
    {
        base.OnHide();
    }
    //物体销毁时执行
    public override void OnDestroy()
    {
        base.OnDestroy();
    }
    #endregion
    #region API Function

    #endregion
    #region UI组件事件
    public void OnNormalAttackButtonClick()
    {
        mHeroLogic.ReleaseNormalAttack();
    }
    #endregion
}
