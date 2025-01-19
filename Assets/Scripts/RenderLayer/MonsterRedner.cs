using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;
public class MonsterRedner : RenderObject
{
    private Animation mAnim;
    private string mCurAnimName;
    private int mMonsterid;
    private MonsterCfg mMonsterCfg;
    private MonsterLogic mMonsterLogic;
    public override void OnCreate()
    {
        base.OnCreate();
        mAnim = transform.GetComponentInChildren<Animation>();
        mMonsterLogic = (logicObject as MonsterLogic);
        mMonsterid = mMonsterLogic.MonsterId;
        mMonsterCfg = ConfigCenter.Instance.GetMonsterCfgById(mMonsterid);
    }
    public override void PlayAnim(string name)
    {
        base.PlayAnim(name);
        if (mAnim == null)
        {
            return;
        }
        //��������ʱ ֻ�ܲ�����������
        if (logicObject.ObjectState == LogicObjectState.Death && !string.Equals(name, "Anim_Dead"))
        {
            return;
        }
        mCurAnimName = name;
        if (!mAnim.GetClip(name))
        {
            return;
        }
        mAnim.Play(name);
    }
    public override string GetCurAnimName()
    {
        return mCurAnimName;
    }
    public override void OnRelease()
    {
        base.OnRelease();
    }
    public override void OnDeath()
    {
        base.OnDeath();
        PlayAnim(AnimationName.Anim_Dead);
        LogicTimerManager.Instance.DelayCall(1.5, () =>
        {
            ZMAssetsFrame.Release(gameObject);
        });
    }
    public override void OnHit(string effectHitObjPath, int survivalTimems, LogicObject source)
    {
        base.OnHit(effectHitObjPath, survivalTimems, source);
        //ͨ�����������ļ������ù������Ϣ��������id������Ѫ�������������ƶ��ٶȡ��ܻ���Ч��������Ч�� Excel
        //���ع����ʱ���ȡ���ã�������ЧҲ�Ƕ����õġ�
        //��ʱ����
        AudioClip audioClip = null;
        if (mMonsterid == 20001)//�粼��
        {
            audioClip = ZMAssetsFrame.LoadAudio(AssetPathConfig.GAME_AUIDO_PATH + "Gebulin/GoblinAttackC.wav");
        }
        else if (mMonsterid == 20005)//֩��
        {
            audioClip = ZMAssetsFrame.LoadAudio(AssetPathConfig.GAME_AUIDO_PATH + "zhizu/NorthrendGhoulWound1.wav");
        }
        if (audioClip != null)
        {
            AudioController.GetInstance().PlaySoundByAudioClip(audioClip, false, 2);
        }
    }

    public override void Damage(int damageValue, DamageSource source)
    {
        base.Damage(damageValue, source);
        UIModule.Instance.GetWindow<BattleWindow>().ShowMonsterDamage(mMonsterCfg, gameObject.GetInstanceID(), mMonsterLogic.HP + damageValue, damageValue);
    }
}
