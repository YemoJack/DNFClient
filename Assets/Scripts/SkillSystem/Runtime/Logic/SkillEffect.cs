using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//技能特效相关
public partial class Skill
{


    /// <summary>
    /// 特效字典 key为特效配置的HashCode Value为特效对象
    /// </summary>
    private Dictionary<int,GameObject> mEffectDic = new Dictionary<int,GameObject>();



    public void OnLogicFrameUpdateEffect()
    {
   


        if(mSkillData.effectCfgList != null && mSkillData.effectCfgList.Count > 0)
        {
            foreach(var item in mSkillData.effectCfgList)
            {
                if(item.skillEffect != null && mCurLogicFrame == item.triggerFrame)
                {
                    DestroyEffect(item);
                    //技能特效生成
                    GameObject effectObj = GameObject.Instantiate(item.skillEffect);//TODO...1.通过Editor获取当前对象的一个路径，2.把特效名字改成字符串
                    effectObj.transform.position = Vector3.zero;
                    effectObj.transform.rotation = Quaternion.identity;
                    effectObj.transform.localScale = Vector3.one;
                    mEffectDic.Add(item.GetHashCode(),effectObj);
                }

                if(mCurLogicFrame == item.endFrame)
                {
                    DestroyEffect(item);
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
        GameObject effect = null;
        int hashCode = item.GetHashCode();
        mEffectDic.TryGetValue(hashCode, out effect);
        if(effect != null)
        {
            
            mEffectDic.Remove(hashCode);
            GameObject.Destroy(effect);
        }
    }



}