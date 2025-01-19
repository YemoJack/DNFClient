using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HideMonoScript]
[System.Serializable]
public class SkillConfig
{
    [HideInInspector]
    public bool showStockPileData = false;//�Ƿ���ʾ������������
    [HideInInspector]
    public bool showSkillGuide = false;//�Ƿ���ʾ������������


    [LabelText("����ͼ��"),LabelWidth(0.1f),PreviewField(70,ObjectFieldAlignment.Left),SuffixLabel("����ͼ��")]
    public Sprite skillIcon;

    [LabelText("����id")]
    public int skillid;
    [LabelText("��������")]
    public string skillName;//��������
    [LabelText("������������")]
    public int needMgicValue = 100;//������������
    [LabelText("����ǰҡʱ��")]
    public int skillShakeBeforeTimeMs;//����ǰҡʱ��
    [LabelText("���ܹ�������ʱ��")]
    public int skillAttackDurationMs;//���ܹ�������ʱ��
    [LabelText("���ܺ�ҡʱ��")]
    public int skillShakeArfterMs;//���ܺ�ҡʱ��
    [LabelText("������ȴʱ��")]
    public int skillCDTimeMs;//������ȴʱ��
    [LabelText("��������"),OnValueChanged("OnSKillTypeChange")]
    public SKillType skillType;//��������

    [LabelText("�����׶�����(����һ�׶δ���ʱ�䲻Ϊ0����յ�ʱ��Ϊ��������ʱ��)"),ShowIf("showStockPileData")]
    public List<StockPileStageData> stockPileStageData;//����������������
    [LabelText("����������Ч"),ShowIf("showSkillGuide")]
    public GameObject skillGuideObj;//����������Ч
    [LabelText("����������Χ"), ShowIf("showSkillGuide")]
    public float skillGuideRange;//����������Χ

    [LabelText("��ϼ���id(�ν���һ�����ܵ�id)"), Tooltip("���磺����A �ɼ��� C B D���")]
    public int ComobinationSkillid;
    //������Ⱦ���
    [LabelText("����������Ч"),TitleGroup("������Ⱦ","����Ӣ����Ⱦ���ݻ��ڿ�ʼ�ͷż���ʱ����"),OnValueChanged("GetObjectPath")]
    public GameObject skillHitEffect;//����������Ч
    [ReadOnly]
    public string skillHitEffectPath;
    [LabelText("���ܻ�����Ч���ʱ��"), TitleGroup("������Ⱦ", "����Ӣ����Ⱦ���ݻ��ڿ�ʼ�ͷż���ʱ����")]
    public int hitEffectSurvivalTimeMs = 100;//���ܻ�����Ч���ʱ��
    [LabelText("����������Ч"), TitleGroup("������Ⱦ", "����Ӣ����Ⱦ���ݻ��ڿ�ʼ�ͷż���ʱ����")]
    public AudioClip skillHitAudio;//����������Ч
    [LabelText("�Ƿ���ʾ��������"), TitleGroup("������Ⱦ", "����Ӣ����Ⱦ���ݻ��ڿ�ʼ�ͷż���ʱ����")]
    public bool showSkillPortrait;//�Ƿ���ʾ��������
    [LabelText("�����������"), TitleGroup("������Ⱦ", "����Ӣ����Ⱦ���ݻ��ڿ�ʼ�ͷż���ʱ����"),ShowIf("showSkillPortrait")]
    public GameObject skillProtraitObj;//�����������
    [LabelText("��������"), TitleGroup("������Ⱦ", "����Ӣ����Ⱦ���ݻ��ڿ�ʼ�ͷż���ʱ����")]
    public string skillDes;//��������
#if UNITY_EDITOR

    public void GetObjectPath(GameObject obj)
    {
        skillHitEffectPath = UnityEditor.AssetDatabase.GetAssetPath(obj);
        Debug.Log("skillHitEffectPath:" + skillHitEffectPath);
    }
#endif
    /// <summary>
    /// �������͸ı�
    /// </summary>
    public void OnSKillTypeChange(SKillType sKillType)
    {
        showStockPileData = sKillType == SKillType.StockPile;
        showSkillGuide = sKillType == SKillType.PosGuide;
    }
}

public enum SKillType
{
    [LabelText("�����ã�˲�����ܣ�")] None, 
    [LabelText("�����ͼ���")] Chnat,//�����ͼ���
    [LabelText("�����ͼ���")] Ballistic,//�����ͼ���
    [LabelText("��������")] StockPile,//�������� 
    [LabelText("λ����������")] PosGuide,//λ����������
}

/// <summary>
/// �����׶�����
/// </summary>
[System.Serializable]
public class StockPileStageData
{
    [LabelText("�����׶�id")]
    public int stage;//�����׶�id
    [LabelText("��ǰ�����׶δ����ļ���id")]
    public int skillid;//��ǰ�����׶δ����ļ���id
    [LabelText("��ǰ�׶δ�����ʼʱ��")]
    public int startTimeMs;//��ǰ�׶δ�����ʼʱ��
    [LabelText("��ǰ�׶ν���ʱ��")]
    public int endTimeMs;//��ǰ�׶ν���ʱ��
}