using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;

namespace ZMGC.Battle
{
    public class BattleWorld : World
    {
        /// <summary>
        /// 英雄控制器
        /// </summary>
        public HeroLogicCtrl HeroLogicCtrl { get; private set; }
        /// <summary>
        /// 怪物控制器
        /// </summary>
        public MonsterLogicCtrl MonsterLogicCtrl { get; private set; }


        public override void OnCretae()
        {
            base.OnCretae();

            HeroLogicCtrl = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>();
            MonsterLogicCtrl = BattleWorld.GetExitsLogicCtrl<MonsterLogicCtrl>();

            HeroLogicCtrl.InitHero();
            MonsterLogicCtrl.InitMonster();


            Debug.Log("BattleWorld  OnCretae>>>");
            
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void OnDestroyPostProcess(object args)
        {
            base.OnDestroyPostProcess(args);

        }

    }

}