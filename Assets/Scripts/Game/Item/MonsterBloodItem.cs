using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZM.AssetFrameWork;

public class MonsterBloodItem : MonoBehaviour
{
    public Image headImage;
    public Image monsterTypeImage;
    public MultipleBloodBars bloodBars;
    public Text nameText;

    private MonsterCfg mMonsterCfg;
    private int monsterid;
    public int curShowMonsterInsid;

    public void InitBloodData(MonsterCfg monsterCfg,int curHp,int insid)
    {
        curShowMonsterInsid = insid;
        //1.ʹ��Ѫ��ȥ��ʼ��Ѫ��
        bloodBars.InitBlood(curHp);
        //2.ͨ������ȥ���ع����ͷ��͹��������ͼ��
        headImage.sprite = ZMAssetsFrame.LoadSprite(AssetPathConfig.GAME_TEXTURES_PATH+ "HeadIcon/"+ monsterCfg.id);
        monsterTypeImage.sprite = ZMAssetsFrame.LoadPNGAtlasSprite(AssetPathConfig.GAME_TEXTURES_PATH + "BttlePEV/p_UI_Battle_Pve", GetMonsterTypeName(monsterCfg));
        nameText.text = monsterCfg.name;
    }
    public void Damage(int damageHp)
    {
        bloodBars.ChangeBlood(damageHp);
    }
    public string GetMonsterTypeName(MonsterCfg monsterCfg)
    {
        switch (monsterCfg.type)
        {
            case MonsterType.Normal:
                return "UI_Battle_Pve_Tubiao_Putong";
            case MonsterType.Elite:
                return "UI_Battle_Pve_Tubiao_Jingying";
            case MonsterType.Boss:
                return "UI_Battle_Pve_Tubiao_Lingzhu";
        }
        return "";
    }
}
