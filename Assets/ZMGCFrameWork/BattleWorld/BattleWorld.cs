using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;

namespace ZMGC.Battle
{
    public class BattleWorld : World
    {
        /// <summary>
        /// 逻辑帧累计运行时间
        /// </summary>
        private float mAccLogicRuntime;
        /// <summary>
        /// 下一个逻辑帧开始的时间
        /// </summary>
        private float mNextLogicFrameTime;
        /// <summary>
        /// 逻辑帧动画缓动时间
        /// </summary>
        private float logicDeltaTime;


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

            UIModule.PopUpWindow<BattleWindow>();
            Debug.Log("BattleWorld  OnCretae>>>");
            
        }


        public override void OnUpdate()
        {
            base.OnUpdate();

            //逻辑帧累计运行时间累加
            mAccLogicRuntime += Time.deltaTime;

            //当前逻辑帧时间如果大于下一个逻辑帧时间，就需要更新逻辑帧
            //追帧操作
            //控制帧数，保证所有设备逻辑帧帧数的一致性，并进行追帧操作
            while(mAccLogicRuntime > mNextLogicFrameTime)
            {
                //更新逻辑帧
                OnLogicFrameUpdate();
                //计算下一个逻辑帧的运行时间
                mNextLogicFrameTime += LogicFrameConfig.LogicFrameInterval;
                //逻辑帧id进行自增
                LogicFrameConfig.LogicFrameid++;
            }
           
            logicDeltaTime = (mAccLogicRuntime + LogicFrameConfig.LogicFrameInterval - mNextLogicFrameTime) / LogicFrameConfig.LogicFrameInterval;


        }

        /// <summary>
        /// 逻辑帧更新（后期通过服务端调用）
        /// </summary>
        private void OnLogicFrameUpdate()
        {
            HeroLogicCtrl.OnLogicFrameUpdate();
            MonsterLogicCtrl.OnLogicFrameUpdate();
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