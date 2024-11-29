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
        //�첽���س���
        AsyncOperation operation = SceneManager.LoadSceneAsync("Battle");
        //Ĭ�ϲ�����
        operation.allowSceneActivation = false;

        float curProgress = 0;
        float maxProgress = 100;
        
        //Unity�������ؽ���Ϊ0����0.9
        while (curProgress<90)
        {
            curProgress = operation.progress * 100f;
            //ͨ���¼��ѵ�ǰ�����׳�
            UIEventControl.DispensEvent(UIEventEnum.ScencProgressUpdate,curProgress);
            yield return null;
        }

        while (curProgress<maxProgress)
        {
            curProgress ++;
            //�ȿ�֡Ϊ����UI��Ⱦ�и���
            UIEventControl.DispensEvent(UIEventEnum.ScencProgressUpdate, curProgress);
            yield return null;
        }
        //�����
        operation.allowSceneActivation = true;
        yield return null;

        //������ɫ
        UIModule.Instance.DestroyAllWindow();
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
