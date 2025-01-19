using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class SkillAudioConfig  
{
    [AssetList]
    [BoxGroup("��Ч�ļ�"),PreviewField(70,ObjectFieldAlignment.Left),OnValueChanged("OnAudioChange")]
    public AudioClip skillAudio;

    [LabelText("��Ч�ļ�����"),BoxGroup("��Ч�ļ�"),ReadOnly,GUIColor("green")]
    public string audioName;

    [BoxGroup("��������"),LabelText("����֡"),GUIColor("green")]
    public int triggerFrame;

    [ToggleGroup("isLoop","�Ƿ�ѭ��")]
    public bool isLoop = false;

    [ToggleGroup("isLoop","����֡")]
    public int endFrame;

    public void OnAudioChange()
    {
        if (skillAudio!=null)
        {
            audioName = skillAudio.name;
        }
    }
}
