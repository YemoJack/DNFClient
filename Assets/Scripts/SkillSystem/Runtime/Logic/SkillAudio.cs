using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Skill
{
    
    /// <summary>
    /// ��Ч�߼�֡����
    /// </summary>
    public void OnLogicFrameUpdateAudio()
    {
        if (mSkillData.audioCfgList!=null&&mSkillData.audioCfgList.Count>0)
        {
            foreach (var item in mSkillData.audioCfgList)
            {
                //�Ƿ�ﵽ�˵�ǰ��Ч���õĲ��Ŵ���֡
                if (item.triggerFrame==mCurLogicFrame)
                {
                    //������Ч
                    AudioController.GetInstance().PlaySoundByAudioClip(item.skillAudio,item.isLoop,100);
                }

                //�Ƿ���ѭ����Ч�����Ҵﵽ��ѭ����Ч�Ľ���֡
                if (item.isLoop&&item.endFrame==mCurLogicFrame)
                {
                    //ֹͣ��ǰ��Ч��ѭ������  
                    AudioController.GetInstance().StopSound(item.skillAudio);
                }
            }
        }
    }
    /// <summary>
    /// ���Ż�����Ч
    /// </summary>
    public void PlayHitAudio()
    {
        //������Ч
        AudioController.GetInstance().PlaySoundByAudioClip(mSkillData.skillCfg.skillHitAudio, false, 100);
    }
}
