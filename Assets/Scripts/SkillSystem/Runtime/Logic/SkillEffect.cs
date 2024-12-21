using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SkillEffect
public partial class Skill
{


    /// <summary>
    /// 特效字典 key为特效配置的HashCode Value为特效对象
    /// </summary>
    private Dictionary<int,SkillEffectLogic> mEffectDic = new Dictionary<int, SkillEffectLogic>();



    public void OnLogicFrameUpdateEffect()
    {
   


        if(mSkillData.effectCfgList != null && mSkillData.effectCfgList.Count > 0)
        {
            foreach(var item in mSkillData.effectCfgList)
            {
                if(item.skillEffect != null && mCurLogicFrame == item.triggerFrame)
                {
                    DestroyEffect(item);
                    Transform parent = null;
                    if(item.isSetTransParent)
                    {
                        //获取左手或右手节点
                        parent = mSkillCreater.RenderObj.GetTransParent(item.transParent);
                    }

                    //技能特效生成
                    GameObject effectObj = GameObject.Instantiate(item.skillEffect,parent);//TODO...1.通过Editor获取当前对象的一个路径，2.把特效名字改成字符串
                    effectObj.transform.position = Vector3.zero;
                    effectObj.transform.rotation = Quaternion.identity;
                    effectObj.transform.localScale = Vector3.one;
                    //创建技能特效渲染层
                    SkillEffectRender effectRender = effectObj.GetComponent<SkillEffectRender>();
                    if (effectRender == null)
                    {
                        effectRender = effectObj.AddComponent<SkillEffectRender>();
                    }
                    //创建技能特效逻辑层
                    SkillEffectLogic effectLogic = new SkillEffectLogic(LogicObjectType.Effect,item, effectRender, mSkillCreater);
                    effectRender.SetLogicObject(effectLogic,item.effectPosType != EffectPosType.Zero);
                    mEffectDic.Add(item.GetHashCode(),effectLogic);
                }

                if(mCurLogicFrame == item.endFrame)
                {
                    //技能特效结束，开始销毁
                    DestroyEffect(item);
                    continue;
                }

                SkillEffectLogic effectLogicObj = null;
                //更新特效逻辑层逻辑帧
                if(mEffectDic.TryGetValue(item.GetHashCode(), out effectLogicObj))
                {
                    effectLogicObj.OnLogicFrameEffectUpdate(this,mCurLogicFrame);
                }


            }
        }


    }


    /// <summary>
    /// 销毁对应配置生成的特效
    /// </summary>
    /// <param name="item"></param>
    public void DestroyEffect(SkillEffectConfig item)
    {
        SkillEffectLogic effect = null;
        int hashCode = item.GetHashCode();
        mEffectDic.TryGetValue(hashCode, out effect);
        if(effect != null)
        {
            
            mEffectDic.Remove(hashCode);
            effect.OnDestroy();
        }
    }



}