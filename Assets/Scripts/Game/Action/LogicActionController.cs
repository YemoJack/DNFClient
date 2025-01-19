using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;
public class LogicActionController:Singleton<LogicActionController>
{
    /// <summary>
    /// �ж��б�
    /// </summary>
    private List<ActionBehaviour> mActionList = new List<ActionBehaviour>();

    /// <summary>
    /// ��ʼ�����ж�
    /// </summary>
    /// <param name="action"></param>
    public void RunAciton(ActionBehaviour action)
    {
        action.actionFinsih = false;
        mActionList.Add(action);
    }
    /// <summary>
    /// �߼�֡����֡
    /// </summary>
    public void OnLogicFrameUpdate()
    {
        //�Ƴ��Ѿ���ɵ��ж�
        for (int i = mActionList.Count-1; i >=0 ; i--)
        {
           ActionBehaviour action=  mActionList[i];
            if (action.actionFinsih)
            {
                action.OnActionFinish();
                RemoveAction(action);
            }
        }
        //�����߼�֡
        foreach (var item in mActionList)
        {
            item.OnLogicFrameUpdate();
        }
    }
    /// <summary>
    /// �Ƴ���Ӧ���ж�
    /// </summary>
    /// <param name="action"></param>
    public void RemoveAction(ActionBehaviour action)
    {
        mActionList.Remove(action);
    }
    /// <summary>
    /// �ű���Դ�ͷ�
    /// </summary>
    public void OnDestroy()
    {
        mActionList.Clear();
    }
}
