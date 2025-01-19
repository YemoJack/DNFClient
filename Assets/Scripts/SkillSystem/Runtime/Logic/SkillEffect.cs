using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;
public partial class Skill
{
    /// <summary>
    /// ��Ч�����ֵ� keyΪ��Ч���õ�HashCode��ValueΪ���ɵĶ�Ӧ����Ч
    /// </summary>

    private Dictionary<int, SkillEffectLogic> mEffectDic = new Dictionary<int, SkillEffectLogic>();
    public void OnLogicFrameUpdateEffect()
    {
        if (mSkillData.effectCfgList != null && mSkillData.effectCfgList.Count > 0)
        {
            foreach (var item in mSkillData.effectCfgList)
            {

                if (item.skillEffect != null && mCurLogicFrame == item.triggerFrame)
                {
                    DestroyEffect(item);
                    Transform parent = null;
                    if (item.isSetTransParent)
                    {
                        //��ȡ���ֻ����ֽڵ�
                        parent = mSkillCreater.RenderObj.GetTransParent(item.transParent);
                    }
                    //������Ч���ɴ��� 
                    GameObject effectObj = ZMAssetsFrame.Instantiate(item.skillEffectPath, parent, Vector3.zero, Vector3.one, Quaternion.identity);
                    effectObj.transform.localPosition = Vector3.zero;
                    effectObj.transform.localScale = Vector3.one;
                    effectObj.transform.rotation = Quaternion.identity;
                    effectObj.transform.localEulerAngles = Vector3.zero;
                    //����������Ч��Ⱦ��
                    SkillEffectRender effectRender = effectObj.GetComponent<SkillEffectRender>();
                    if (effectRender == null)
                        effectRender = effectObj.AddComponent<SkillEffectRender>();
                    //����������Ч�߼���
                    SkillEffectLogic effectLogic = new SkillEffectLogic(LogicObjectType.Effect, item, effectRender, mSkillCreater,this);
                    effectRender.SetLoigcObject(effectLogic,item.effectPosType!= EffectPosType.Zero);
                    mEffectDic.Add(item.GetHashCode(), effectLogic);
                }

                if (mCurLogicFrame == item.endFrame&&!item.isAttachAction)
                {
                    //������Ч��������ʼ����
                    DestroyEffect(item);
                    continue;
                }
                SkillEffectLogic effectLogicObj = null;
                //������Ч�߼����߼�֡
                if (mEffectDic.TryGetValue(item.GetHashCode(),out effectLogicObj)&& effectLogicObj!=null)
                {
                    effectLogicObj.OnLogicFrameEffectUpdate(this,mCurLogicFrame);
                }
            }
        }
    }

    /// <summary>
    /// ���ٶ�Ӧ�������ɵ���Ч
    /// </summary>
    /// <param name="item"></param>
    public void DestroyEffect(SkillEffectConfig item)
    {
        SkillEffectLogic effect = null;
        int hashCode = item.GetHashCode();
        mEffectDic.TryGetValue(hashCode, out effect);
        if (effect != null)
        {
            mEffectDic.Remove(hashCode);
            effect.OnDestroy();
        }
    }
    /// <summary>
    /// �ͷ�������Ч��Դ
    /// </summary>
    public void ReleaseAllEffect()
    {
        foreach (var item in mSkillData.effectCfgList)
        {
            if (!item.isAttachAction)
            {
                DestroyEffect(item);
            }
        }
    }
}
 