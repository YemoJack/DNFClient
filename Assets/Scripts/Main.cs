using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZMGC.Hall;

public class Main : MonoBehaviour
{

    public static Main Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        UIModule.Instance.Initialize();

        WorldManager.CreateWorld<HallWorld>();

       
        DontDestroyOnLoad(gameObject);
    }


    public void LoadSceneAsync()
    {
        UIModule.Instance.PopUpWindow<LoadingWindow>();
        StartCoroutine(AsyncLoadScene());
    }

    IEnumerator AsyncLoadScene()
    {
        //异步加载场景
        AsyncOperation operation = SceneManager.LoadSceneAsync("Battle");
        //默认不激活
        operation.allowSceneActivation = false;

        float curProgress = 0;
        float maxProgress = 100;
        
        //Unity场景加载进度为0——0.9
        while (curProgress<90)
        {
            curProgress = operation.progress * 100f;
            //通过事件把当前进度抛出
            UIEventControl.DispensEvent(UIEventEnum.ScencProgressUpdate,curProgress);
            yield return null;
        }

        while (curProgress<maxProgress)
        {
            curProgress ++;
            //等空帧为了让UI渲染有更新
            UIEventControl.DispensEvent(UIEventEnum.ScencProgressUpdate, curProgress);
            yield return null;
        }
        //激活场景
        operation.allowSceneActivation = true;
        yield return null;

        //创建角色
        UIModule.Instance.DestroyAllWindow();
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
