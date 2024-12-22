using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;

/// <summary>
/// 渲染对象
/// </summary>
public class RenderObject : MonoBehaviour
{

    public LogicObject logicObject;

    protected Vector2 mRenderDir;


    /// <summary>
    /// 位置插值速度
    /// </summary>
    protected float mSmoothPosSpeed = 10;


    private bool isUpdatePosAndRot = true;

    public void SetLogicObject(LogicObject logicObject,bool isUpdatePosAndRot = true)
    {
        this.logicObject = logicObject;
        this.isUpdatePosAndRot = isUpdatePosAndRot;
        //初始化
        transform.position = logicObject.LogicPos.ToVector3();

        if(!isUpdatePosAndRot)
            transform.localPosition = Vector3.zero;
        UpdateDir();
    }

    /// <summary>
    /// 渲染层脚本创建
    /// </summary>
    public virtual void OnCreate()
    {
        
    }

    /// <summary>
    /// 渲染层脚本释放
    /// </summary>
    public virtual void OnRelease()
    {
        
    }



    /// <summary>
    /// Unity渲染帧 根据程序配置，渲染帧一般为30，60，120
    /// </summary>
    protected virtual void Update()
    {
        UpdatePosition();
        UpdateDir();
    }

    /// <summary>
    /// 通用的位置更新
    /// </summary>
    private void UpdatePosition()
    {
        if (!isUpdatePosAndRot)
            return;
        transform.position = Vector3.Lerp(transform.position,logicObject.LogicPos.ToVector3(),Time.deltaTime * mSmoothPosSpeed);
    }

    /// <summary>
    /// 通用的方向更新
    /// </summary>
    private void UpdateDir()
    {
        if (!isUpdatePosAndRot)
            return;
        //transform.rotation = Quaternion.Euler(logicObject.LogicDir.ToVector3());

        //mRenderDir.x = logicObject.LogicXAxis >= 0 ? 0 : -20;
        mRenderDir.y = logicObject.LogicXAxis >= 0 ? 0 : 180;
        transform.localEulerAngles = mRenderDir;

        //TODO...调整角色左右的朝向
        //transform.localScale = new Vector3(logicObject.LogicXAxis >= 0 ? 1 : -1,1,1);

    }

    public virtual void PlayAnim(AnimationClip clip)
    {

    }


    /// <summary>
    /// 伤害
    /// </summary>
    /// <param name="damageValue"></param>
    /// <param name="damageSource">伤害来源</param>
    public virtual void Damage(int damageValue , DamageSource damageSource)
    {
        GameObject damageItemObj = ZMAssetsFrame.Instantiate(AssetPathConfig.GAME_PREFABS + "DamageItem/DamageText", null);
        DamageTextItem item = damageItemObj.GetComponent<DamageTextItem>();
        item.ShowDamageText(damageValue,this);
    }

    public virtual void OnHit(GameObject effectHitObj, int surcicalTimems, LogicActor source)
    {
        GameObject hitEffect = GameObject.Instantiate(effectHitObj);
        hitEffect.transform.position = transform.position;
        hitEffect.transform.localScale = source.LogicXAxis >0 ?Vector3.one :new Vector3(-1,1,1);
        GameObject.Destroy(hitEffect,surcicalTimems/1000f);
    }


    public virtual Transform GetTransParent(TransParentType parentType)
    {
        return null;
    }


}
