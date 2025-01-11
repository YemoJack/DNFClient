using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRender : RenderObject
{

    private Animation mAnim;

    public override void OnCreate()
    {
        base.OnCreate();
        mAnim = GetComponentInChildren<Animation>();
    }

    public override void PlayAnim(string name)
    {
        base.PlayAnim(name);
        if(mAnim == null)
        {
            return;
        }
        //怪物死亡时，只能播放死亡动画
        if(logicObject.ObjectState == LogicObjectState.Death && !string.Equals(name, "Anim_Dead"))
        {
            return;
        }
        mAnim.Play(name);
    }



    public override void OnRelease()
    {
        base.OnRelease();
    }


}
