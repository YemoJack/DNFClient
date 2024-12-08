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



}
