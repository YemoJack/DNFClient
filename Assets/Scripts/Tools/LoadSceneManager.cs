using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZM.AssetFrameWork;
public class LoadSceneManager : MonoSingleton<LoadSceneManager>
{
    public void LoadSceneAsync(string sceneName,Action LoadSceneFinish)
    {
        UIModule.Instance.PopUpWindow<LoadingWindow>();
        StartCoroutine(AsyncLoadScene(sceneName, LoadSceneFinish));
    }
    IEnumerator AsyncLoadScene(string sceneName, Action LoadSceneFinish)
    {
        //�첽���س���
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        //Ĭ�ϲ�����������
        operation.allowSceneActivation = false;

        //��ǰ���ؽ���
        float curProgress = 0;
        //�����ؽ���
        float maxProgress = 100;
        //Unity�������ؽ���ֻ���0-0.9  ��ʣ��0.1��Ҫ����ʹ�ô�����һ������
        while (curProgress < 90)
        {
            curProgress = operation.progress * 100.0f;
            //ͨ��һ���¼��ѵ�ǰ�����׳���
            UIEventControl.DispensEvent(UIEventEnum.SceneProgressUpdate, curProgress);
            yield return null;
        }

        while (curProgress < maxProgress)
        {
            curProgress++;
            UIEventControl.DispensEvent(UIEventEnum.SceneProgressUpdate, curProgress);
            //��һ����֡��Ϊ����UI����Ⱦ�Ĺ���
            yield return null;
        }
        //�����Ѽ�����ɵĳ���
        operation.allowSceneActivation = true;
        yield return null;
        LoadSceneFinish?.Invoke();
     
        
    }
}
