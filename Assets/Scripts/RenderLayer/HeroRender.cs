using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixMath;
public class HeroRender : RenderObject
{
    private HeroLogic mHeroLogic;
    /// <summary>
    /// ��ɫ����������
    /// </summary>
    private Animation mAnim;
    /// <summary>
    /// ��ǰҡ�����뷽��
    /// </summary>
    public Vector3 mInputDir;
    /// <summary>
    /// ���ֽڵ�
    /// </summary>
    public Transform Left_Hand_RootTrans;
    /// <summary>
    /// ���ֽڵ�
    /// </summary>
    public Transform Right_Hand_RootTrans;
    /// <summary>
    /// ����������Ч����
    /// </summary>
    private GameObject mSkillGuideEffectObj;

    public override void OnCreate()
    {
        base.OnCreate();
        mHeroLogic = logicObject as HeroLogic;
        JoystickUGUI.OnMoveCallBack += OnJoyStickMove;
        mAnim = transform.GetComponent<Animation>();
    }

    public override void OnRelease()
    {
        base.OnRelease();
        JoystickUGUI.OnMoveCallBack -= OnJoyStickMove;
    }
    /// <summary>
    /// ҡ���ƶ�����
    /// </summary>
    public void OnJoyStickMove(Vector3 inputDir)
    {
        mInputDir = inputDir;
        //�߼�����
        FixIntVector3 logicDir = FixIntVector3.zero;

        if (inputDir != Vector3.zero)
        {
            logicDir.x = inputDir.x;
            logicDir.y = inputDir.y;
            logicDir.z = inputDir.z;
        }
        //��Ӣ���߼���ֱ���������֡�¼� û�з��������µĲ��Դ���
        mHeroLogic.InputLoigcFrameEvent(logicDir);
    }

    public override void Update()
    {
        base.Update();
        //�ж���û�м������ͷ��У�����м������ͷ��У��ǲ��������ƶ��ʹ���������
        if (mHeroLogic.releaseingSkillList.Count == 0)
        {
            //�ж�ҡ�������Ƿ���ֵ���룬���û���У����Ŵ��������������ֵ�������ƶ�����
            if (mInputDir.x == 0 && mInputDir.z == 0)
            {
                PlayAnim("Anim_Idle02");
            }
            else
            {
                PlayAnim("Anim_Run");
            }
        }

    }
    /// <summary>
    /// ���Ž�ɫ����
    /// </summary>
    /// <param name="animName"></param>
    public void PlayAnim(string animName)
    {
        mAnim.CrossFade(animName, 0.2f);
    }
    /// <summary>
    /// ͨ�������ļ����Ŷ���
    /// </summary>
    /// <param name="clip"></param>
    public override void PlayAnim(AnimationClip clip)
    {
        base.PlayAnim(clip);

        if (mAnim.GetClip(clip.name) == null)
        {
            mAnim.AddClip(clip, clip.name);
        }
        mAnim.clip = clip;
        PlayAnim(clip.name);
    }
    /// <summary>
    /// ��ȡ���ڵ�
    /// </summary>
    /// <param name="parentType"></param>
    /// <returns></returns>
    public override Transform GetTransParent(TransParentType parentType)
    {
        if (parentType == TransParentType.LeftHand)
        {
            return Left_Hand_RootTrans;
        }
        else if (parentType == TransParentType.RightHand)
        {
            return Right_Hand_RootTrans;
        }
        return null;
    }


    public void InitSkillGuide(int skillid)
    {
        if (mSkillGuideEffectObj == null)
        {
            Skill skill = mHeroLogic.GetSKill(skillid);
            mSkillGuideEffectObj = GameObject.Instantiate(skill.SKillCfg.skillGuideObj);
            mSkillGuideEffectObj.transform.localScale = Vector3.one;
        }
    }
    /// <summary>
    /// ���¼�������
    /// </summary>
    /// <param name="sKillGuideType">������������</param>
    /// <param name="skillid">����id</param>
    /// <param name="isPreass">��ָ�Ƿ���</param>
    /// <param name="pos">ҡ�˵�λ��</param>
    /// <param name="skillRange">����������Χ</param>
    public void UpdateSkillGuide(SKillGuideType sKillGuideType, int skillid, bool isPreass, Vector3 pos, float skillRange)
    {
        //��ʼ��������Ч
        InitSkillGuide(skillid);
        //����������Чλ��
        switch (sKillGuideType)
        {
            case SKillGuideType.Click:
                break;
            case SKillGuideType.LongPress:
                break;
            case SKillGuideType.Position:
                Vector3 skillGuidePos= transform.position + pos;
                //���Ƶ�ǰλ�õ�z�� ���ܳ�����ͼ
                skillGuidePos = new Vector3(skillGuidePos.x,0,Mathf.Clamp(skillGuidePos.z,-1,8.6f));
                mSkillGuideEffectObj.transform.localPosition = skillGuidePos;
                break;
            case SKillGuideType.Dirction:
                break;
        }
    }
    public void OnGuideRelease()
    {
        if (mSkillGuideEffectObj!=null)
        {
            GameObject.Destroy(mSkillGuideEffectObj);
        }
    }
}
