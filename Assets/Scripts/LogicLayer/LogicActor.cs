using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 逻辑对象类
/// </summary>
public partial class LogicActor : LogicObject
{

    public override void OnCreate()
    {
        base.OnCreate();
        InitActorSkill();
    }




    public override void OnLogicFrameUpdate()
    {
        base.OnLogicFrameUpdate();
        //更新移动
        OnLogicFrameUpdateMove();
        //更新技能
        OnLogicFrameUpdateSkill();
        //更新重力
        OnLogicFrameUpdateGravity();

    }

    public void PlayAnim(AnimationClip clip)
    {
        RenderObj.PlayAnim(clip);
    }



    public override void OnDestroy()
    {
        base.OnDestroy();
    }

}
