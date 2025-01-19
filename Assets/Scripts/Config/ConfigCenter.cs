using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;

public class ConfigCenter:Singleton<ConfigCenter>
{ 
    //Ӣ�������ֵ� key=Ӣ��id valueΪӢ����������
    private Dictionary<int, HeroDataCfg> mHeroDtaCfgDic;
    //���������ֵ� key=����id valueΪ������������
    private Dictionary<int, MonsterCfg> mMonsterCfgDic;
    public void InitGameCfg()
    {
        LoadMonsterCfg();
        LoadHeroCfg();
    }

    #region ���ü���
    private void LoadMonsterCfg()
    {
        mMonsterCfgDic = new Dictionary<int, MonsterCfg>();
        //���������ļ�
         TextAsset textAsset = ZMAssetsFrame.LoadTextAsset(AssetPathConfig.GAME_DATA_PATH+ "MonsterCfg.json");
        if (textAsset==null)
        {
            Debug.LogError("LoadMonsterCfg Failed, textAsset is Null !");
            return;
        }
        //�����л�Json
        List<MonsterCfg> monsterCfgList =JsonConvert.DeserializeObject<List<MonsterCfg>>(textAsset.text);
        foreach (var item in monsterCfgList)
        {
            mMonsterCfgDic.Add(item.id,item);
        }
        Debug.LogError("LoadMonsterCfg Success, Count:"+mMonsterCfgDic.Count);
    }
    private void LoadHeroCfg()
    {
        mHeroDtaCfgDic = new Dictionary<int, HeroDataCfg>();
        //���������ļ�
        TextAsset textAsset = ZMAssetsFrame.LoadTextAsset(AssetPathConfig.GAME_DATA_PATH + "HeroDataCfg.json");
        if (textAsset == null)
        {
            Debug.LogError("LoadHeroCfg Failed, textAsset is Null !");
            return;
        }
        //�����л�Json
        List<HeroDataCfg> heroCfgList = JsonConvert.DeserializeObject<List<HeroDataCfg>>(textAsset.text);
        foreach (var item in heroCfgList)
        {
            mHeroDtaCfgDic.Add(item.id, item);
        }
        Debug.LogError("LoadHeroCfg Success, Count:" + mHeroDtaCfgDic.Count);
    }
    #endregion

    #region ���û�ȡ
    /// <summary>
    /// ͨ������id��ȡ������������
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public MonsterCfg GetMonsterCfgById(int id)
    {
        MonsterCfg monsterCfg = null;
        mMonsterCfgDic.TryGetValue( id,out monsterCfg);
        return monsterCfg;
    }
    /// <summary>
    /// ͨ������id��ȡӢ����������
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public HeroDataCfg GetHeroCfgById(int id)
    {
        HeroDataCfg heroCfg = null;
        mHeroDtaCfgDic.TryGetValue(id, out heroCfg);
        return heroCfg;
    }
    #endregion
}
