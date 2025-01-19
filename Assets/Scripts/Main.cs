using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZMGC.Battle;
using ZMGC.Hall;
using ZM.AssetFrameWork;
public class Main : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        //��ʼ����Դ������
        ZMAssetsFrame.Instance.InitFrameWork();
        //��ʼ��UI���
        UIModule.Instance.Initialize();
        WorldManager.CreateWorld<HallWorld>();
        //���������ٵ�ǰ�ڵ�
        DontDestroyOnLoad(gameObject);
        //�������1
        //LogicRandom random1 = new LogicRandom(10);
        //string randomResult = "logicRamdom:";
        //for (int i = 0; i < 11; i++)
        //{
        //    randomResult += random1.Range(1,360)+",";
        //}
        //Debug.Log(randomResult);

        ////�������2
        //LogicRandom random2 = new LogicRandom(10);
        //string randomResult2 = "logicRamdom:";
        //for (int i = 0; i < 11; i++)
        //{
        //    randomResult2 += random2.Range(1, 360) + ",";
        //}
        //Debug.Log(randomResult2);
        //1.randomResult randomResult2 ��������һ�£�
        //2.randomResult randomResult2 ��������ȫһ�£�
    }
    /// <summary>
    /// ��Դ��ѹ���֮�����ã�
    /// </summary>
    public void StartGame()
    {
        
    }
 

    // Update is called once per frame
    void Update()
    {
        WorldManager.OnUpdate();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>().HeroLogic.velocity = new FixMath.FixIntVector3(0, 6, 0);
            BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>().HeroLogic.isAddForce=true;
        }
    }
}
