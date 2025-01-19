using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FixMath;

public class SkillItem : MonoBehaviour
{
    public Text cdText;
    public Image iconImage;
    public Image cdMaskImage;
    //����ҡ�˽ű�
    public SKillItem_JoyStick skillJoyStick;

    private Skill mSkillData;
    private LogicActor mSkillCreater;
    private HeroRender mHeroRender;
    //�Ƿ���뼼��CD
    private bool mIsEnterSkillCD;
    //�Ѿ���ȴ��ʱ��
    private float mAlreadyCDTime;
    //������ȴʱ��
    private float mSkillCDTime;
    /// <summary>
    /// ���ü�������
    /// </summary>
    /// <param name="skillData">��������</param>
    /// <param name="logicActor">�����ͷ���</param>
    public void SetItemSkillData(Skill skillData,LogicActor logicActor)
    {
        mSkillData = skillData;
        mSkillCreater = logicActor;
        mHeroRender = logicActor.RenderObj as HeroRender;
        //��ʼ������ҡ������
        skillJoyStick.InitSkillData(GetSkillGuideType(skillData.SKillCfg.skillType),skillData.skillid,skillData.SKillCfg.skillGuideRange);
        skillJoyStick.OnReleaseSkill += OnTriggerSKill;
        skillJoyStick.OnSkillGuide += OnUpdateSkillGuide;
        iconImage.sprite = skillData.SKillCfg.skillIcon;
        cdText.gameObject.SetActive(false);
        cdMaskImage.gameObject.SetActive(false);
    }
  
    /// <summary>
    /// ������Ӧ�ļ��� ���ͷ�ҡ��ʱ������
    /// </summary>
    /// <param name="sKillGuide">������������</param>
    /// <param name="skillPos">���ܴ���λ��</param>
    /// <param name="skillId">���ܴ���id</param>
    public void OnTriggerSKill(SKillGuideType sKillGuide, Vector3 skillPos, int skillId)
    {
        if (sKillGuide== SKillGuideType.Click)
        {
            mSkillCreater.ReleaseSKill(skillId,releaseSkillCallBack: OnReleaseSkillCallBack);
        }
        else if (sKillGuide== SKillGuideType.LongPress)
        {
            //������������
            mSkillCreater.TriggerStockPileSkill(skillId);
        }
        else if (sKillGuide== SKillGuideType.Position)
        {
            //ȷ����������λ��һ���ڵ�����
            skillPos.y = 0;
            //ָ��λ�ü��� TODO �ͷŵ�ǰ�ļ���
            mSkillCreater.ReleaseSKill(skillId,mSkillCreater.LogicPos+new FixIntVector3(skillPos),OnReleaseSkillCallBack);
            mHeroRender.OnGuideRelease();
         }
    }
    /// <summary>
    /// ���¼��������ص� (��ҡ��һֱ���µ�ʱ�����)
    /// </summary>
    /// <param name="sKillGuide">������������</param>
    /// <param name="isCancel">�Ƿ�ȡ��</param>
    /// <param name="skillPos">�����ͷ�λ��</param>
    /// <param name="skillId">����id</param>
    /// <param name="skillDirDis">���ܰ뾶����</param>
    public void OnUpdateSkillGuide(SKillGuideType sKillGuide, bool isCancel, Vector3 skillPos, int skillId, float skillDirDis)
    {
        if (sKillGuide== SKillGuideType.LongPress)
        {
            //��������
            mSkillCreater.ReleaseSKill(skillId, releaseSkillCallBack: OnReleaseSkillCallBack);
        }
        else if (sKillGuide == SKillGuideType.Position)
        {
            //TODO ��ɫ
            mHeroRender.UpdateSkillGuide(sKillGuide, skillId, isCancel, skillPos, skillDirDis);
        }
    }
    /// <summary>
    /// �ͷż��ܻص�
    /// </summary>
    /// <param name="isReleaseSuccess"></param>
    public void OnReleaseSkillCallBack(bool isReleaseSuccess)
    {
        if (isReleaseSuccess)
        {
            EnterSKillCD();
        }
    }
    /// <summary>
    /// ���뼼��CD
    /// </summary>
    public void EnterSKillCD()
    {
        cdText.gameObject.SetActive(true);
        cdMaskImage.gameObject.SetActive(true);
        mIsEnterSkillCD = true;
        //��ȡ������ȴʱ��
        mSkillCDTime =mAlreadyCDTime = mSkillData.SKillCfg.skillCDTimeMs/1000;
        cdText.text = mSkillCDTime.ToString();
        int cdTime = mSkillData.SKillCfg.skillCDTimeMs / 1000;
        //�����߼�֡��ʱ�������µ�ǰ������ȴʱ��
        LogicTimerManager.Instance.DelayCall(1,()=> {
            //������ȴ�߼�
            cdTime--;
            if (cdTime<=0)
            {
                //������ȴ����
                cdText.gameObject.SetActive(false);
                cdMaskImage.gameObject.SetActive(false);
                mIsEnterSkillCD = false;
            }
            else
            {
                cdText.text = cdTime.ToString();
            }
        }, cdTime);
    }
    private void Update()
    {
        if (mIsEnterSkillCD)
        {
            cdMaskImage.fillAmount = (mAlreadyCDTime -= Time.deltaTime) / mSkillCDTime;
        }
    }
    /// <summary>
    /// ���ݼ������ͻ�ȡ������������
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public SKillGuideType GetSkillGuideType(SKillType type)
    {
        SKillGuideType sKillGuide = SKillGuideType.Click;
        if (type == SKillType.StockPile)
        {
            sKillGuide = SKillGuideType.LongPress;
        }
        else if (type == SKillType.Ballistic|| type== SKillType.Chnat||type== SKillType.None)
        {
            sKillGuide = SKillGuideType.Click;
        }
        else if (type== SKillType.PosGuide)
        {
            sKillGuide = SKillGuideType.Position;
        }
        return sKillGuide;
    }
    public void OnDestroy()
    {
        skillJoyStick.OnReleaseSkill -= OnTriggerSKill;
        skillJoyStick.OnSkillGuide -= OnUpdateSkillGuide;
    }
}
