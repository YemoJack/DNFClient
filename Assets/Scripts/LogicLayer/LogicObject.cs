using FixIntPhysics;
using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 逻辑对象基类
/// LogicObject 同时表示 怪物和英雄同时具有的基础属性
/// 只负责最基础的属性和接口的提供，不负责具体方法的实现
/// </summary>
public abstract class LogicObject 
{
    private FixIntVector3 logicPos;     //逻辑对象逻辑位置
    private FixIntVector3 logicDir;     //逻辑对象朝向
    private FixIntVector3 logicAngle;   //逻辑对象旋转角度
    private FixInt logicMoveSpeed = 3;      //逻辑对象移动速度
    private FixInt logicXAxis = 1;       //逻辑轴向
    private bool isActive;              //当前逻辑对象是否激活


    //公开属性
    public FixIntVector3 LogicPos { get { return logicPos; } set { logicPos = value; } }     //逻辑对象逻辑位置
    public FixIntVector3 LogicDir{ get { return logicDir; } set { logicDir = value; } }       //逻辑对象朝向
    public FixIntVector3 LogicAngle { get { return logicAngle; } set { logicAngle = value; } }     //逻辑对象旋转角度
    public FixInt LogicMoveSpeed { get { return logicMoveSpeed; } set { logicMoveSpeed = value; } }        //逻辑对象移动速度
    public FixInt LogicXAxis { get { return logicXAxis; }set { logicXAxis = value; } }         //逻辑轴向
    public bool IsActive { get { return isActive; } set { isActive = value; } }                //当前逻辑对象是否激活

    /// <summary>
    /// 渲染对象
    /// </summary>
    public RenderObject RenderObj { get;protected set; }
    /// <summary>
    /// 定点数碰撞库
    /// </summary>
    public FixIntBoxCollider Collider { get; protected set; }

    /// <summary>
    /// 逻辑对象
    /// </summary>
    public LogicObjectState ObjectState { get; set; }
    /// <summary>
    /// 逻辑对象类型
    /// </summary>
    public LogicObjectType ObjectType { get; set; }

    /// <summary>
    /// 逻辑对象行为状态
    /// </summary>
    public LogicObjectActionState ActionState { get; set; }


    /// <summary>
    /// 初始化接口
    /// </summary>
    public virtual void OnCreate()
    {

    }

    /// <summary>
    /// 逻辑帧更新接口
    /// </summary>
    public virtual void OnLogicFrameUpdate()
    {

    }

    /// <summary>
    /// 逻辑对象释放接口
    /// </summary>
    public virtual void OnDestroy()
    {

    }


}

public enum LogicObjectActionState
{
    Idle,//待机
    Move,//移动中
    SKillReleaseSkilling,//释放技能中
    Floating,//浮空中
    Hiting,//受击中
    StockPileing,//蓄力中
}

public enum LogicObjectType
{
    Hero,
    Monster,
    Effect,
}

public enum LogicObjectState
{
    Survival,//存活中
    Death,//死亡
}


