using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
/// <summary>
/// �ж�����
/// </summary>
public enum MoveActionType
{
    [LabelText("ָ��Ŀ��λ��")]TargetPos,
    [LabelText("����λ��")] GuidePos,
    [LabelText("�������ƶ�")] BezierPos,
}
/// <summary>
/// �ж���ɺ�Ĳ���
/// </summary>
public enum MoveActionFinishOpation
{ 
    None,
    Skill,
    Buff,
}

[System.Serializable]
public class SkillActionConfig
{
    //�Ƿ���ʾ�ƶ�λ��
    private bool mIsShowMovePos;
    //�Ƿ���ʾ�ƶ���ɲ���
    private bool mIsShowFinishParam;
    //�Ƿ���ʾ����������
    private bool mIsShowBezierPos;

    [LabelText("����֡")]
    public int triggerFrame;
    [LabelText("�ƶ���ʽ"),OnValueChanged("OnMoveActionTypeChange")]
    public MoveActionType moveActionType;
    [LabelText("��ߵ�λ��"),ShowIf("mIsShowBezierPos")]
    public Vector3 heightPos;
    [LabelText("�ƶ�λ��"), ShowIf("mIsShowMovePos")]
    public Vector3 movePos;
    [LabelText("�ƶ�����ʱ��(���װ�)")]
    public int durationMs;

    [LabelText("�ƶ���ɲ���"), OnValueChanged("OnMoveActionFinishOpationChange")]
    public MoveActionFinishOpation actionFinishOpation;

    [LabelText("��������"), ShowIf("mIsShowFinishParam")]
    public List<int> actionFinishidList;

    public void OnMoveActionTypeChange(MoveActionType value)
    {
        mIsShowMovePos = value == MoveActionType.TargetPos || value == MoveActionType.BezierPos;
        mIsShowBezierPos = value == MoveActionType.BezierPos;
    }
    public void OnMoveActionFinishOpationChange(MoveActionFinishOpation value)
    {
        mIsShowFinishParam = value != MoveActionFinishOpation.None;
    }
}
