using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZMGC.Battle;
using ZMGC.Hall;
using ZM.AssetFrameWork;

public class Main : MonoBehaviour
{

    public static Main Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        //初始化资源管理框架
        ZMAssetsFrame.Instance.InitFrameWork();
        //初始化UI模块
        UIModule.Instance.Initialize();

        //创建大厅世界
        WorldManager.CreateWorld<HallWorld>();

       
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 资源解压完成后调用
    /// </summary>
    public void StartGame()
    {

    }


  
}
