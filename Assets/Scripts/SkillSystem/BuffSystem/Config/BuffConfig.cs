using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff����", menuName = "Buff����", order = 0)]
[System.Serializable]
public class BuffConfig : ScriptableObject
{
    [LabelText("buffͼ��"),LabelWidth(0.1f),PreviewField(70,ObjectFieldAlignment.Left),SuffixLabel("Buffͼ��")]
    public Sprite buffIcon;//buffͼ�� 
    [LabelText("buffID")]
    public int buffid;//BuffΨһid
    [LabelText("Buff����")]
    public string buffName;
    [LabelText("�ӳٴ���ʱ��")]
    public int buffDelay;//��buff  1 2 3 ��֮�󴥷�buff��Ӧ��Ч�� ��������˺� �����ѣ
    [LabelText("�������")]
    public int buffIntervalms;//��Buff ÿ��n��֮��ظ�һ������ֵ
    [LabelText("Buff����ʱ��(0��ʾһ�� -1��ʾbuffʱ������ ֱ��ս������)")]
    public int buffDurationms;//0��ʾһ�� -1��ʾbuffʱ������ ֱ��ս������  >0Buff��ʵ��Чʱ��
    [LabelText("Buff����")]
    public BuffType buffType;//��ʾ��ǰbuff��ʲôbuff ����ѣ ��Ĭ��
    [LabelText("����Ŀ��")]
    public BuffAttachType attachType;
    [LabelText("����λ��")]
    public BuffPosType buffPosType;
    [LabelText("Buff�˺�����")]
    public DamageType damageType;
    [LabelText("Buff�˺�����")]
    public int damageRate;
    [LabelText("Buff ��ֵ����")]
    public List<BuffParams> buffParamsList;
    [LabelText("ץȡ����")]
    public TargetGrabData targetGrabData;//ץȡ���ݣ���ѹ���ץȡĳһ��λ��


    [LabelText("buff������Ч"),TitleGroup("���ܱ���","���еı������ݻ���Buff�ͷ�ʱ��Buff����ʱ����")]
    public AudioClip buffAudio;
    [LabelText("Buff������Ч"), TitleGroup("���ܱ���", "���еı������ݻ���Buff�ͷ�ʱ��Buff����ʱ����")]
    public BuffEffectConfig effectConfig;
    [LabelText("Buff������Ч"), TitleGroup("���ܱ���", "���еı������ݻ���Buff�ͷ�ʱ��Buff����ʱ����"),OnValueChanged("GetObjectPath")]
    public GameObject buffHitEffectObj;
    [ReadOnly]
    public string buffHitEffectObjPath;
    [LabelText("Buff��������"), TitleGroup("���ܱ���", "���еı������ݻ���Buff�ͷ�ʱ��Buff����ʱ����")]
    public ObjectAnimationState buffTriggerAnim= ObjectAnimationState.None;//buff�����Ķ���
    [LabelText("�˺�/Ŀ������")]
    public TargetConfig targetConfig;//buff�˺����/Ŀ������

    [Title("Buff������"),HideLabel,MultiLineProperty(5)]
    public string buffDes;

#if UNITY_EDITOR
    public void GetObjectPath(GameObject obj)
    {
        buffHitEffectObjPath = UnityEditor.AssetDatabase.GetAssetPath(obj);
        Debug.Log("skillHitEffectPath:" + buffHitEffectObjPath);
    }
    public void SaveAsset()
    {
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
    }
#endif
}
[System.Serializable,TabGroup("Ŀ������")]
public class TargetConfig
{
    [LabelText("�Ƿ�����")]
    public bool isOpen = false;
    [LabelText("����Ŀ��"), ShowIf("isOpen")]
    public TargetType targetType;//����Ŀ��
    [LabelText("�˺��������"),ShowIf("isOpen")]
    public SkillDamageConfig damageCfg;//�˺�����
}

 //Eidtor 1.����
 //

/// <summary>
/// ��ʾ��ǰbuff����ʱ����Ҫ���ŵĶ���
/// </summary>
public enum ObjectAnimationState
{
    [LabelText("������")] None,
    [LabelText("�ܻ�")] BeHit,
    [LabelText("��ֱ")] Stiff,
}

[System.Serializable]
public class BuffEffectConfig
{
    [LabelText("��Ч����"),OnValueChanged("GetObjectPath")]
    public GameObject effect;
    [ReadOnly]
    public string effectPath;
    [LabelText("��Ч��������")]
    public EffectAttachType attachType;
    [LabelText("��Чλ������")]
    public BuffEffectPosType effectPosType;

#if UNITY_EDITOR
    public void GetObjectPath(GameObject obj)
    {
        effectPath = UnityEditor.AssetDatabase.GetAssetPath(obj);
        Debug.Log("effectPath:" + effectPath);
    }
#endif
}
[LabelText("��Чλ������")]
public enum BuffEffectPosType
{
    [LabelText("������")] None,
    [LabelText("����Ŀ��")] targetFollow,
    [LabelText("Ŀ��λ��")] TargetPos,
}
[LabelText("��Ч��������")]
public enum EffectAttachType
{
    [LabelText("������")] None,
    [LabelText("����")] Conter,
    [LabelText("�ֲ�")] Hand,
}
[System.Serializable]
public class TargetGrabData
{
    [LabelText("ץȡ����Ŀ��λ��")]
    public Vector3 garbMoveTargetPos;
    [LabelText("�ƶ���ץȡλ������ʱ��")]
    public int moveTimems;//��ʾҪץȡ��Ŀ���ƶ���ץȡĿ��λ����Ҫ���
}

[System.Serializable]
public class BuffParams
{
    [LabelText("����"),PropertyTooltip("���磺��ɵ��˺��������˵ľ���")]
    public float value;
    [LabelText("��������")]
    public string des;
}
[LabelText("λ������")]
public enum BuffPosType
{
    [LabelText("������")] None,
    [LabelText("����Ŀ��")] FollowTarget,//����Ŀ��
    [LabelText("����Ŀ��λ��")] HitTargetPos,//���ӵ�����λ��
    [LabelText("ʩ����λ��")] ReleaserPos,//UIҡ������λ��
    [LabelText("����λ��")] UIInputPos,//UIҡ������λ��
}
[LabelText("��������")]
public enum BuffAttachType
{
    [LabelText("������")] None,
    [LabelText("ʩ����")] Creator,//���ӵ�ʩ��������
    [LabelText("ʩ��Ŀ��")] Target,//���ӵ�ʩ��Ŀ������
    [LabelText("ʩ����λ��")] Creator_Pos,//���ӵ�ʩ��������λ��
    [LabelText("ʩ����λ��")] Target_Pos,//���ӵ�ʩ��Ŀ������λ��
    [LabelText("����λ��")] Guide_Pos,//���ڳ�����ͨ������ҡ��ѡ�е�λ��
}
[LabelText("buff����")]
public enum BuffType 
{
    [LabelText("������")] None =0,
    [LabelText("����")] Repel,
    [LabelText("����")] Floating,
    [LabelText("��ֱ")] Stiff,
    [LabelText("Ⱥ��Ѫ���޸�")] HP_Modify_Group,

    [LabelText("���������޸�")] MoveSpeed_Modify_Single,

    [LabelText("ץȡ")] Grab,
    [LabelText("��������")] IgnoreGravity,
    [LabelText("�����ƶ�")] AllowMove,
    [LabelText("������ת��")] NotAllowDir,
}