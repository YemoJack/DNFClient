using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectRender : RenderObject
{
    protected override void Update()
    {
        base.Update();


    }


    public override void OnRelease()
    {
        base.OnRelease();
        GameObject.Destroy(gameObject);
    }
}
