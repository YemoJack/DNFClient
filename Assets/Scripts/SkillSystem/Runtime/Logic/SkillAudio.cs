using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SkillAudio
public partial class Skill
{
    
    /// <summary>
    /// 音效逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdateAudio()
    {
        if(mSkillData.audioCfgList != null && mSkillData.audioCfgList.Count > 0)
        {
            foreach(var item in mSkillData.audioCfgList)
            {
                //是否达到了当前音效配置的播放触发帧
                if(item.triggerFrame == mCurLogicFrame)
                {
                    //播放音效
                    AudioController.GetInstance().PlaySoundByAudioClip(item.skillAudio, item.isLoop, 100);
                }

                //是否是循环音效，并且到达了循环音效的结束帧
                if(item.isLoop && item.endFrame == mCurLogicFrame)
                {
                    //停止当前音效的循环播放 
                    AudioController.GetInstance().StopSound(item.skillAudio);
                }
            }
        }
    }

    /// <summary>
    /// 播放击中音效
    /// </summary>
    public void PlayHitAudio()
    {
        //播放音效
        AudioController.GetInstance().PlaySoundByAudioClip(mSkillData.skillCfg.skillHitAudio,false, 100);
    }



}
