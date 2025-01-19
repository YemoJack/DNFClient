using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillEffectConfig 
{
    [AssetList][LabelText("������Ч����")][PreviewField(70, ObjectFieldAlignment.Left),OnValueChanged("GetObjectPath")]
    public GameObject skillEffect;//������Ч
    [ReadOnly]
    public string skillEffectPath;
    [LabelText("����֡")]
    public int triggerFrame;//����֡
    [LabelText("����֡")]
    public int endFrame;//����֡
    [LabelText("��Чƫ��λ��")]
    public Vector3 effectOffsetPos;//��Чƫ��λ��
    [LabelText("��Чλ������")]
    public EffectPosType effectPosType;//��Чλ������
    [ToggleGroup("isSetTransParent","�Ƿ�������Ч���ڵ�")]
    public bool isSetTransParent = false;//�Ƿ�������Ч���ڵ�
    [ToggleGroup("isSetTransParent", "�ڵ�����")]
    public TransParentType transParent;//���ڵ�����
    [ToggleGroup("isAttachDamage", "�Ƿ񸽼��˺�")]
    public bool isAttachDamage=false;
    [ToggleGroup("isAttachDamage", "�Ƿ񸽼��˺�")]
    public SkillDamageConfig damageConfig;

    [ToggleGroup("isAttachAction", "�Ƿ񸽼��ж�")]
    public bool isAttachAction=false;
    [ToggleGroup("isAttachAction", "�Ƿ񸽼��ж�")]
    public SkillActionConfig actionConfig;

    [HideInInspector]
    public GameObject GameEffectObj;//��Ϸ��Ч�������
#if UNITY_EDITOR

    public void GetObjectPath(GameObject obj)
    {
        skillEffectPath= UnityEditor.AssetDatabase.GetAssetPath(obj);
        Debug.Log("skillEffectPath:"+ skillEffectPath);
    }

    //Editorģʽ�¿�¡����Ч����
    private GameObject mCloneEffect;
    private AnimationAgnet mAnimAgent;
    private ParticlesAgent mParticleAgent;
    //��ǰ�߼�֡
    private int mCurLogicFrame = 0;
    /// <summary>
    /// ��ʼ���ż���
    /// </summary>
    public void StartPlaySkill()
    {
        DestroyEffect(); 
        mCurLogicFrame = 0;
    }
    public void SkillPause()
    {
        DestroyEffect();
    }
    /// <summary>
    /// ���ż��ܽ���
    /// </summary>
    public void PlaySkillEnd()
    {
        DestroyEffect();
    }
    /// <summary>
    /// �߼�֡����
    /// </summary>
    public void OnLogicFrameUpdate()
    {
        if (mCurLogicFrame==triggerFrame)
        {
            CreateEffect();
        }
        else if (mCurLogicFrame == endFrame)
        {
            DestroyEffect();
        }
        mCurLogicFrame++;
    }
    /// <summary>
    /// ������Ч
    /// </summary>
    public void CreateEffect()
    {
        if (skillEffect!=null)
        {
            mCloneEffect= GameObject.Instantiate(skillEffect);
            mCloneEffect.transform.position = SkillComplierWindow.GetCharaterPos();
            //TODO ��Editorģʽ�����ļ���������Ч�������Զ����ţ���Ҫ����ͨ��������в���
            mAnimAgent = new AnimationAgnet();
            mAnimAgent.InitPlayAnim(mCloneEffect.transform);

            mParticleAgent = new ParticlesAgent();
            mParticleAgent.InitPlayAnim(mCloneEffect.transform);
        }
    }
    /// <summary>
    /// ������Ч
    /// </summary>
    public void DestroyEffect()
    {
        if (mCloneEffect!=null)
        {
            GameObject.DestroyImmediate(mCloneEffect);
        }
        if (mAnimAgent!=null)
        {
            mAnimAgent.OnDestroy();
            mAnimAgent = null;
        }
        if (mParticleAgent != null)
        {
            mParticleAgent.OnDestroy();
            mParticleAgent = null;
        }
    }


#endif

}

public enum TransParentType
{
    [LabelText("������")] None,
    [LabelText("����")] LeftHand,//����
    [LabelText("����")] RightHand,//����
}

public enum EffectPosType
{
   [LabelText("�����ɫλ�úͷ���")] FollowPosDir,//�����ɫλ�úͷ���
    [LabelText("�����ɫ����")] FollowDir,//ֻ�����ɫ����
    [LabelText("��Ļ����λ��")] ConterPos,//��Ļ����λ��
    [LabelText("����λ��")] GuidePos,//����λ��
    [LabelText("������Ч�ƶ�λ��")] FollwEffectMovePos,//������Ч�ƶ�λ��
    [LabelText("λ�ù���")] Zero,//λ�ù���
}