using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum MoveActionFinishOpation
{
    None,
    Skill,
    Buff,
    
}

public enum MoveActionType
{
    [LabelText("指定目标位置")] TargetPos,
    [LabelText("引导位置")] GuidPos,
    [LabelText("贝塞尔移动")] BeziePos,
}


[System.Serializable]
public class SkillActionConfig
{
    //是否显示移动位置
    private bool mIsShowMovePos;
    //是否显示移动完成参数
    private bool mIsShowFinishParam;
    //是否显示贝塞尔数据
    private bool mIsShowBeziePos;
    [LabelText("触发帧")]
    public int triggerFrame;
    [LabelText("移动方式"),OnValueChanged("OnMoveActionTypeChange")]
    public MoveActionType moveActionType;
    [LabelText("最高点位置"),ShowIf("mIsShowBeziePos")]
    public Vector3 heightPos;
    [LabelText("移动位置"), ShowIf("mIsShowMovePos")]
    public Vector3 movePos;
    [LabelText("移动所需时间MS")]
    public int durationMs;
    [LabelText("移动完成操作"), OnValueChanged("OnMoveActionFinishOpationChange")]
    public MoveActionFinishOpation actionFinishOpation;
    [LabelText("触发参数"),ShowIf("mIsShowFinishParam")]
    public List<int> actionFinishidList;

    public void OnMoveActionTypeChange(MoveActionType value)
    {
        mIsShowMovePos = value == MoveActionType.TargetPos || value == MoveActionType.BeziePos;
        mIsShowBeziePos = value == MoveActionType.BeziePos;
    }


    public void OnMoveActionFinishOpationChange(MoveActionFinishOpation value)
    {
        mIsShowFinishParam = value != MoveActionFinishOpation.None;
    }



}
