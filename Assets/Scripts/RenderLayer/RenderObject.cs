using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;

/// <summary>
/// ��Ⱦ����
/// </summary>
public class RenderObject : MonoBehaviour
{
    /// <summary>
    /// �߼�����
    /// </summary>
    public LogicObject logicObject;
    /// <summary>
    /// λ�ò�ֵ�ٶ�
    /// </summary>
    protected float mSmoothPosSpeed = 10;

    protected bool mIsUpdatePosAndRotation = true;
    protected Vector2 mRenderDir;
    public void SetLoigcObject(LogicObject logicObj,bool isUpdatePosAndRotation=true)
    {
        logicObject = logicObj;
        mIsUpdatePosAndRotation = isUpdatePosAndRotation;
        //��ʼ��λ��
        transform.position = logicObj.LogicPos.ToVector3();
        if (mIsUpdatePosAndRotation == false)
            transform.localPosition = Vector3.zero;
        UpdateDir();
    }
    /// <summary>
    /// ��Ⱦ��ű�����
    /// </summary>
    public virtual void OnCreate()
    {

    }
    /// <summary>
    /// ��Ⱦ��ű��ͷ�
    /// </summary>
    public virtual void OnRelease()
    {

    }
    /// <summary>
    /// Unity������Ⱦ֡�����ݳ������ã���Ⱦ֡һ��һ��Ϊ30֡����60֡�Լ�120֡ 
    /// </summary>
    public virtual void Update()
    {
        UpdatePosition();
        UpdateDir();
    }
    /// <summary>
    ///ͨ�õ�λ�ø����߼�
    /// </summary>
    public virtual void UpdatePosition()
    {
        if (mIsUpdatePosAndRotation == false)
        {
            return;
        }
        //���߼�λ������ֵ������������Ⱦ�����ƶ�
        transform.position = Vector3.Lerp(transform.position, logicObject.LogicPos.ToVector3(), Time.deltaTime * mSmoothPosSpeed);
    }
    /// <summary>
    /// ͨ�õķ�������߼�
    /// </summary>
    public virtual void UpdateDir()
    {
        if (mIsUpdatePosAndRotation == false)
        {
            return;
        }
        //mRenderDir.x = logicObject.LogicXAxis >= 0 ? 0 : -20;
        mRenderDir.y = logicObject.LogicXAxis >= 0 ? 0 : 180;
        transform.localEulerAngles = mRenderDir;
    }
    public virtual void OnDeath()
    {

    }
    public virtual void PlayAnim(AnimationClip clip)
    {

    }
    public virtual void PlayAnim(string name)
    {

    }

    public virtual string GetCurAnimName()
    {
        return "";
    }
    /// <summary>
    /// �˺�
    /// </summary>
    /// <param name="damageValue">�˺�ֵ</param>
    /// <param name="source">�˺���Դ</param>
    public virtual void Damage(int damageValue, DamageSource source)
    {
        GameObject damageItemObj = ZMAssetsFrame.Instantiate(AssetPathConfig.GAME_PREFABS + "DamageItem/DamageText", null);
        DamageTextItem item = damageItemObj.GetComponent<DamageTextItem>();
        item.ShowDamageText(damageValue, this);
    }
    public virtual void OnHit(string effectHitObjPath, int survivalTimems, LogicObject source)
    {
        if (!string.IsNullOrEmpty(effectHitObjPath))
        {
            //GameObject hitEffctObj= GameObject.Instantiate(effectHitObj);
            GameObject hitEffctObj = ZMAssetsFrame.Instantiate(effectHitObjPath, null);
            hitEffctObj.transform.position = source.RenderObj.transform.position; //�������߼���Ϊ�˱���ͳһ����ֱ��ʹ����Ⱦλ��
            hitEffctObj.transform.localScale = source.LogicXAxis > 0 ? Vector3.one : new Vector3(-1,1,1);
            //GameObject.Destroy(hitEffctObj, survivalTimems*1.0f/1000);
            LogicTimerManager.Instance.DelayCall(survivalTimems * 1.0f / 1000, () => {
                ZMAssetsFrame.Release(hitEffctObj);
            });
        }
    }
    public virtual Transform GetTransParent(TransParentType parentType) { return null; }

    /// <summary>
    /// ��ʾ��������
    /// </summary>
    /// <param name="portraitObj"></param>
    public virtual void ShowSkillPortrait(GameObject portraitObj)
    {
        GameObject nPortraitObj = GameObject.Instantiate(portraitObj);
        GameObject.Destroy(nPortraitObj, 3);
    }
}
