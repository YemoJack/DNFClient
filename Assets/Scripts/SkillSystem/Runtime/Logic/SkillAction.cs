using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Skill 
{
    
    /// <summary>
    /// �ж��߼�֡����
    /// </summary>
    public void OnLogicFrameUpdateAction()
    {
        //�����ж�����
        if (mSkillData.actionCfgList != null && mSkillData.actionCfgList.Count > 0)
        {
            foreach (var item in mSkillData.actionCfgList)
            {
                if (item.triggerFrame==mCurLogicFrame)
                {
                    //�����ж�
                    AddMoveAction(item,mSkillCreater);
                }
            }
        }
    }
    /// <summary>
    /// ����ƶ��ж�
    /// </summary>
    /// <param name="item">�ж�����</param>
    /// <param name="logicMoveObj">�߼��ƶ�����</param>
    public void AddMoveAction(SkillActionConfig item,LogicObject logicMoveObj,Vector3 offset=default(Vector3), Action moveFinish= null,Action moveUpdateCallBack=null)
    {

        void OnActionFinish()
        {
            Debug.Log("MoveToAction Finish");
            moveFinish?.Invoke();
            if (item.actionFinishOpation != MoveActionFinishOpation.None)
            {
                switch (item.actionFinishOpation)
                {
                    case MoveActionFinishOpation.Skill://�ͷź�������
                        foreach (var item in item.actionFinishidList)
                        {
                            mSkillCreater.ReleaseSKill(item);
                        }
                        break;
                    case MoveActionFinishOpation.Buff://���һ��buff
                        sKillGuidePos = logicMoveObj.LogicPos;
                        foreach (var buffid in item.actionFinishidList)
                        {
                            BuffSystem.Instance.AttachBuff(buffid, mSkillCreater, mSkillCreater, this);
                        }
                        // 1.����ָ���ص����� 2.������ײ��� 3.�ܶԶ�������˺���
                        // Ⱥ�������޸�Buff
                        break;
                }
            }
        }

        FixIntVector3 movePos = new FixIntVector3(item.movePos);
        FixIntVector3 targetPos;
        FixIntVector3 startPos = logicMoveObj.LogicPos;
        targetPos = logicMoveObj.LogicPos + movePos* logicMoveObj.LogicXAxis; //-1 1
                                                                              //�����ƶ�����
        MoveType moveType = MoveType.target;
        if (item.moveActionType == MoveActionType.TargetPos)
        {
            if (movePos.x != FixInt.Zero && movePos.y == FixInt.Zero && movePos.z == FixInt.Zero)
            {
                moveType = MoveType.X;
            }
            else if (movePos.x == FixInt.Zero && movePos.y != FixInt.Zero && movePos.z == FixInt.Zero)
            {
                moveType = MoveType.Y;
            }
            else if (movePos.x == FixInt.Zero && movePos.y == FixInt.Zero && movePos.z != FixInt.Zero)
            {
                moveType = MoveType.Z;
            }
        }
        //����������λ���ƶ��߼�
        else if (item.moveActionType == MoveActionType.GuidePos)
        {
            //Ŀ��λ��
            targetPos = sKillGuidePos;
            //��ʼλ��
            startPos = targetPos + mSkillCreater.LogicXAxis * new FixIntVector3(offset);
            startPos.y = FixIntMath.Abs(startPos.y);
        }
        else if (item.moveActionType == MoveActionType.BezierPos)
        {
            //1.������ʼλ��
            startPos = mSkillCreater.LogicPos + mSkillCreater.LogicXAxis * new FixIntVector3 (offset);
            startPos.y = FixIntMath.Abs(startPos.y);//���õ�ǰ����y<0������ͻ��ܵ�����ȥ
            //2.������ߵ�λ��
            FixIntVector3 heightPosOffset = new FixIntVector3(item.heightPos) * mSkillCreater.LogicXAxis;
            heightPosOffset.y = FixIntMath.Abs(heightPosOffset.y);
            FixIntVector3 heightPos = mSkillCreater.LogicPos + heightPosOffset;
            //3.�������λ��
            FixIntVector3 endPosOffset = new FixIntVector3(item.movePos) * mSkillCreater.LogicXAxis;
            endPosOffset.y= FixIntMath.Abs(endPosOffset.y);
            targetPos = mSkillCreater.LogicPos + endPosOffset;
            //3.ִ�б������˶�
            MoveBezierAction moveBezier = new MoveBezierAction(logicMoveObj,startPos,heightPos,targetPos,item.durationMs, OnActionFinish, moveUpdateCallBack);
            LogicActionController.Instance.RunAciton(moveBezier);
            return;
        }
        MoveToAction action = new MoveToAction(logicMoveObj, startPos, targetPos,item.durationMs, OnActionFinish, moveUpdateCallBack, moveType);
        //��ʼ�ж�
        LogicActionController.Instance.RunAciton(action);
    }
}
