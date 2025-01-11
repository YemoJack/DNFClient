using FixIntPhysics;
using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLogic : LogicActor
{
   
    public int MonsterId;

    public MonsterLogic(int monsterId, RenderObject renderObj,FixIntBoxCollider boxCollider,FixIntVector3 logicPos)
    {
        this.MonsterId = monsterId;
        this.RenderObj = renderObj;
        this.Collider = boxCollider;
        this.LogicPos = logicPos;
        ObjectType = LogicObjectType.Monster;
    }

    public override void OnHit(GameObject effectHitObj, int surcicalTimems, LogicActor source)
    {
        base.OnHit(effectHitObj, surcicalTimems, source);
        LogicXAxis = -source.LogicXAxis;
    }


    public override void Floating(bool upFloating)
    {

        string animName = upFloating ? "Anim_Float_up" : "Anim_Float_down";
        PlayAnim(animName);
        ActionState = LogicObjectActionState.Floating;
    }


    public override void TriggerGround()
    {
        //处理怪物触地
        if(ObjectState != LogicObjectState.Death)
        {
            PlayAnim("Anim_Getup");
        }
        else
        {
            PlayAnim("Anim_Dead");
        }

    }


}
