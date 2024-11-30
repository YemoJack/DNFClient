using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZM.AssetFrameWork;
using ZMGC.Battle;
using ZMGC.Hall;

public class LoadSceneManager : MonoSingleton<LoadSceneManager>
{
    public void LoadSceneAsync(string sceneName,Action FinishLoadScene)
    {
        UIModule.Instance.PopUpWindow<LoadingWindow>();
        StartCoroutine(AsyncLoadScene(sceneName,FinishLoadScene));
    }

    IEnumerator AsyncLoadScene(string sceneName, Action FinishLoadScene)
    {
        //异步加载场景
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        //默认不激活
        operation.allowSceneActivation = false;

        float curProgress = 0;
        float maxProgress = 100;

        //Unity场景加载进度为0——0.9
        while (curProgress < 90)
        {
            curProgress = operation.progress * 100f;
            //通过事件把当前进度抛出
            UIEventControl.DispensEvent(UIEventEnum.ScencProgressUpdate, curProgress);
            yield return null;
        }

        while (curProgress < maxProgress)
        {
            curProgress++;
            //等空帧为了让UI渲染有更新
            UIEventControl.DispensEvent(UIEventEnum.ScencProgressUpdate, curProgress);
            yield return null;
        }
        //激活场景
        operation.allowSceneActivation = true;
        yield return null;

        FinishLoadScene?.Invoke();

        
    }



}
