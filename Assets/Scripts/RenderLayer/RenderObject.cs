using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 渲染对象
/// </summary>
public class RenderObject : MonoBehaviour
{

    public LogicObject logicObject;

    /// <summary>
    /// 位置插值速度
    /// </summary>
    protected float mSmoothPosSpeed;


    public void SetLogicObject(LogicObject logicObject)
    {
        this.logicObject = logicObject;
        //初始化
        transform.position = logicObject.LogicPos.ToVector3();
    }



    /// <summary>
    /// Unity渲染帧 根据程序配置，渲染帧一般为30，60，120
    /// </summary>
    void Update()
    {
        UpdatePosition();
        UpdateDir();
    }

    /// <summary>
    /// 通用的位置更新
    /// </summary>
    private void UpdatePosition()
    {
        transform.position = Vector3.Lerp(transform.position,logicObject.LogicPos.ToVector3(),Time.deltaTime * mSmoothPosSpeed);
    }

    /// <summary>
    /// 通用的方向更新
    /// </summary>
    private void UpdateDir()
    {
        transform.rotation = Quaternion.Euler(logicObject.LogicDir.ToVector3());
    }
}
