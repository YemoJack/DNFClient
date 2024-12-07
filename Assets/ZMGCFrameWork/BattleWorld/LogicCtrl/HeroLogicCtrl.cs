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

        public void OnCreate()
        {
            Debug.Log("HeroLogicCtrl  OnCreate>>>");
        }

        /// <summary>
        /// 初始化场景中的英雄
        /// </summary>
        public void InitHero()
        {
            ZMAssetsFrame.Instantiate(AssetPathConfig.GAME_PREFABS_HERO + "1000", null);
        }



        public void OnDestroy()
        {

        }

    }
}
