/*---------------------------------
 *Title:UI表现层脚本自动化生成工具
 *Author:ZM 铸梦
 *Date:2024/11/29 16:33:45
 *Description:UI 表现层，该层只负责界面的交互、表现相关的更新，不允许编写任何业务逻辑代码
 *注意:以下文件是自动生成的，再次生成不会覆盖原有的代码，会在原有的代码上进行新增，可放心使用
---------------------------------*/
using UnityEngine.UI;
using UnityEngine;
using ZMUIFrameWork;

public class LoadingWindow : WindowBase
{

    public LoadingWindowDataComponent dataCompt;

    #region 声明周期函数
    //调用机制与Mono Awake一致
    public override void OnAwake()
    {
        mDisableAnim = true;
        dataCompt = gameObject.GetComponent<LoadingWindowDataComponent>();
        dataCompt.InitComponent(this);
        base.OnAwake();
    }
    //物体显示时执行
    public override void OnShow()
    {
        base.OnShow();
        UIEventControl.AddEvent(UIEventEnum.ScencProgressUpdate, OnSceneProgressUpdate);

    }
    //物体隐藏时执行
    public override void OnHide()
    {
        base.OnHide();
        UIEventControl.RemoveEvent(UIEventEnum.ScencProgressUpdate, OnSceneProgressUpdate);
    }
    //物体销毁时执行
    public override void OnDestroy()
    {
        base.OnDestroy();
    }
    #endregion
    #region API Function

    public void OnSceneProgressUpdate(object data)
    {
        float progress = (float)data / 100;
        dataCompt.SliderImage.fillAmount = progress;



    }



    #endregion
    #region UI组件事件
    public void OnCloseButtonClick()
    {

        HideWindow();
    }
    #endregion
}
