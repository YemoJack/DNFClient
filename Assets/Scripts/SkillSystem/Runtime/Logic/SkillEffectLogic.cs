using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectLogic : LogicObject
{

    private LogicActor mSkillCreater;
    private SkillEffectConfig mEffectCfg;

    public SkillEffectLogic(LogicObjectType objType,SkillEffectConfig effectCfg,RenderObject renderObj,LogicActor skillCreater)
    {
        this.mSkillCreater = skillCreater;
        ObjectType = objType;
        RenderObj = renderObj;
        mEffectCfg = effectCfg;
        LogicXAxis = mSkillCreater.LogicXAxis;
        //初始化特效逻辑位置
        if(effectCfg.effectPosType == EffectPosType.FollowPosDir || effectCfg.effectPosType == EffectPosType.FollowDir)
        {
            FixIntVector3 offset = new FixMath.FixIntVector3(effectCfg.effectOffsetPos) * LogicXAxis;
            offset.y = FixIntMath.Abs(offset.y);
            LogicPos = mSkillCreater.LogicPos + offset;
        }
    }


    public override void OnDestroy()
    {
        base.OnDestroy();
        RenderObj.OnRelease();
    }

}
