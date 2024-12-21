using System.Collections.Generic;
using ZM.AssetFrameWork;

public class LogicActionController : Singleton<LogicActionController>
{
    /// <summary>
    /// 行动列表
    /// </summary>
    private List<ActionBehavior> mActionList = new List<ActionBehavior>();

    /// <summary>
    /// 开始进行行动
    /// </summary>
    /// <param name="action"></param>
    public void RunAction(ActionBehavior action)
    {
        action.actionFinish = false;
        mActionList.Add(action);
    }

    /// <summary>
    /// 逻辑帧更新帧
    /// </summary>
    public void OnLogicFrameUpdate()
    {
        //移除已经完成的行动
        for(int i = mActionList.Count - 1; i >= 0; i--)
        {
            ActionBehavior action = mActionList[i];
            if(action.actionFinish)
            {
                action.OnActionFinish();
                RemoveAction(action);
            }
        }

        //更新逻辑帧
        foreach(ActionBehavior action in mActionList)
        {
            action.OnLogicFrameUpdate();
        }

    }

    /// <summary>
    /// 移除对应的行动
    /// </summary>
    /// <param name="action"></param>
    public void RemoveAction(ActionBehavior action)
    {
        mActionList.Remove(action);
    }


    /// <summary>
    /// 资源脚本释放
    /// </summary>
    public void OnDestory()
    {
        mActionList.Clear();
    }

}
