/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2024/11/30 10:16:59
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/


using UnityEngine;
using ZM.AssetFrameWork;

namespace ZMGC.Battle
{
    public class HeroLogicCtrl : ILogicBehaviour
    {

        public HeroLogic HeroLogic {  get; private set; }

        public void OnCreate()
        {
            Debug.Log("HeroLogicCtrl  OnCreate>>>");
        }

        /// <summary>
        /// 初始化场景中的英雄
        /// </summary>
        public void InitHero()
        {
            GameObject heroObj = ZMAssetsFrame.Instantiate(AssetPathConfig.GAME_PREFABS_HERO + "1000", null);
            //获取渲染层对象
            HeroRender renderObj = heroObj.GetComponent<HeroRender>();
            HeroLogic heroLogic = new HeroLogic(1000, renderObj);
            HeroLogic = heroLogic;
            renderObj.SetLogicObject(heroLogic);
            //初始化逻辑层和渲染层
            heroLogic.OnCreate();
            renderObj.OnCreate();
            
        }

        public void OnLogicFrameUpdate()
        {
            HeroLogic.OnLogicFrameUpdate();
        }

        public void OnDestroy()
        {

        }

    }
}
