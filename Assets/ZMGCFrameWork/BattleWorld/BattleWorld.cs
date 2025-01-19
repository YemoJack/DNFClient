using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;

namespace ZMGC.Battle
{
    public class BattleWorld : World
    {
        /// <summary>
        /// �߼�֡�ۼ�����ʱ��
        /// </summary>
        private float mAccLogicRuntime;
        /// <summary>
        /// ��һ���߼�֡��ʼ��ʱ��
        /// </summary>
        private float mNextLogicFrameTime;
        /// <summary>
        /// �߼�֡����ʱ��
        /// </summary>
        public float logicDeltaTime;
        /// <summary>
        /// Ӣ�ۿ�����
        /// </summary>
        public HeroLogicCtrl HeroLogicCtrl { get; private set; }
        /// <summary>
        /// ���������
        /// </summary>
        public MonsterLogicCtrl MonsterLogicCtrl { get; private set; }
        /// <summary>
        /// ���繹����ɺ�ִ��
        /// </summary>
        public override void OnCretae()
        {
            base.OnCretae();
            ConfigCenter.Instance.InitGameCfg();
            HeroLogicCtrl = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>();
            MonsterLogicCtrl = BattleWorld.GetExitsLogicCtrl<MonsterLogicCtrl>();
            HeroLogicCtrl.InitHero();
            MonsterLogicCtrl.InitMonster();

            UIModule.PopUpWindow<BattleWindow>();
            Debug.Log("BattleWorld OnCretae");
            BuffSystem.Instance.OnCreate();
            AudioController.GetInstance().PlayMusicFade(AssetPathConfig.GAME_AUIDO_PATH + "BG/jizhou.mp3", 2);
        }
        /// <summary>
        /// Unity��Ⱦ֡���� ģ���߼�֡����
        /// </summary>
        public override void OnUpdate()
        {
            base.OnUpdate();

            //�߼�֡�ۼ�����ʱ���ۼ�
            mAccLogicRuntime += Time.deltaTime;

            //��ǰ�߼�֡ʱ�����������һ���߼�֡ʱ�䣬����Ҫ�����߼�֡
            //׷֡������
            //����֡������֤�����豸�߼�֡֡����һ���ԣ�������׷֡������
            while (mAccLogicRuntime>mNextLogicFrameTime)
            {
                //�����߼�֡
                OnLogicFrameUpdate();
                //������һ���߼�֡������ʱ��
                mNextLogicFrameTime += LogicFrameConfig.LogicFrameInterval;
                //�߼�֡id��������
                LogicFrameConfig.LogicFrameid++;
            }
            //�߼�֡ 1��15֡ ��Ⱦ֡1��60֡
            //0-1 ---- L
            //mAccLogicRuntime 0.01 LogicFrameInterval 0.066 mNextLogicFrameTime 0.066 /0.066
            // 0.01+0.066-0.066 /0.066 =0.01 /0.066= ��ǰֵ/���ֵ=0-1 ��Ѫ���ļ��������һ���ġ�
            // 0.05 +0.066-0.066 /0.066= 
            logicDeltaTime = (mAccLogicRuntime + LogicFrameConfig.LogicFrameInterval - mNextLogicFrameTime) / LogicFrameConfig.LogicFrameInterval;
        }
        /// <summary>
        /// �߼�֡���£�����ͨ������˽��е��ã�
        /// </summary>
        public void OnLogicFrameUpdate()
        {
            HeroLogicCtrl.OnLogicFrameUpdate();
            MonsterLogicCtrl.OnLogicFrameUpdate();
            LogicActionController.Instance.OnLogicFrameUpdate();
            BuffSystem.Instance.OnLogicFrameUpdate();
            LogicTimerManager.Instance.OnLogicFrameUpdate();
        }
        /// <summary>
        /// ��������ʱִ��
        /// </summary>
        public override void OnDestroy()
        {
            base.OnDestroy();
            LogicActionController.Instance.OnDestroy();
            BuffSystem.Instance.OnDestory();
        }
        /// <summary>
        /// ������ȫ���ٺ�ִ��
        /// </summary>
        /// <param name="args"></param>
        public override void OnDestroyPostProcess(object args)
        {
            base.OnDestroyPostProcess(args);

        }
    }
}