/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2024/12/4 15:18:31
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/
using FixIntPhysics;
using FixMath;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;

namespace ZMGC.Battle
{
    public class MonsterLogicCtrl : ILogicBehaviour
    {

        /// <summary>
        /// 场景中怪物列表
        /// </summary>
        public List<MonsterLogic> monsterList = new List<MonsterLogic>();

        public List<Vector3> monsterPosList = new List<Vector3>() 
        { 
            new Vector3(0, 0, 0),
            new Vector3(-2, 0, 0),
        };

        public int[] monsterIdArry = new int[]
        {
            20001,
            20005
        };


        public void OnCreate()
        {

        }
        /// <summary>
        /// 初始化场景中的怪物
        /// </summary>
        public void InitMonster()
        {
            int index = 0;
            
            foreach(int monsterId in monsterIdArry)
            {
                FixIntVector3 initPos = new FixIntVector3(monsterPosList[index]);
                //生成怪物到场景中
                GameObject monsterObj = ZMAssetsFrame.Instantiate(AssetPathConfig.GAME_PREFABS_MONSTER + monsterId, null);
                

                //处理怪物碰撞数据
                BoxColliderGizmo boxInfo = monsterObj.GetComponent<BoxColliderGizmo>();
                boxInfo.enabled = false;
                //创建定点数碰撞体
                FixIntBoxCollider monsterBox = new FixIntBoxCollider(boxInfo.mSize,boxInfo.mConter);
                monsterBox.SetBoxData( boxInfo.mConter, boxInfo.mSize);
                monsterBox.UpdateColliderInfo(initPos, new FixIntVector3( boxInfo.mSize)); //TODO monsterObj.transform.position 待修改为逻辑位置

                //创建怪物和英雄的逻辑层和渲染层
                MonsterRender monsterRender = monsterObj.GetComponent<MonsterRender>();
                MonsterLogic monsterLogic = new MonsterLogic(monsterId,monsterRender,monsterBox,initPos);
                monsterRender.SetLogicObject(monsterLogic);

                monsterLogic.OnCreate();
                monsterRender.OnCreate();

                monsterList.Add(monsterLogic);

                index++;
            }
            

        }

        public void OnLogicFrameUpdate()
        {
            for (int i = 0;i< monsterList.Count; i++)
            {
                monsterList[i].OnLogicFrameUpdate();
            }
        }

        public void OnDestroy()
        {

        }

    }
}
