using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixIntPhysics;
using FixMath;
using ZMGC.Battle;

public class MonsterLogic : LogicActor
{
    public int MonsterId { get; private set; }

    private FixInt attackRange = 1f;//������Χ

    private FixInt chaseDinstance = 4;//׷����ҵľ�����ֵ

    private LogicActor mChaseTarget;//׷��Ŀ��

    public MonsterLogic(int monsterid,RenderObject renderObject,FixIntBoxCollider boxCollider,FixIntVector3 logicPos)
    {
        MonsterId = monsterid;
        RenderObj = renderObject;
        Collider = boxCollider;
        LogicPos = logicPos;
        ObjectType = LogicObjectType.Monster;
    }

    public override void OnCreate()
    {
        base.OnCreate();
        InitMonsterAttribute();
        mChaseTarget = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>().HeroLogic;
        LogicMoveSpeed = 1;
    }

    public override void OnLogicFrameUpdate()
    {
        base.OnLogicFrameUpdate();
        UpdateAIMove();
    }
    /// <summary>
    /// ���¹����ƶ�AI
    /// </summary>
    public void UpdateAIMove()
    {
        if (ObjectState== LogicObjectState.Death)
            return;
        //��ȡĿ��λ��
        FixIntVector3 targetPos = mChaseTarget.LogicPos;
        //������������֮��ķ��������;���
        FixIntVector3 directionToPlayer = (targetPos - LogicPos).normalized;
        FixInt dis = FixIntVector3.Distance(LogicPos,targetPos);
        if (dis<=attackRange)
        {
            //���Խ��й�����
            if (ActionSate== LogicObjectActionState.Idle)
            {
                //PlayAnim(AnimationName.Anim_Idle);
                //�����˾��Ĺ���Ӧ����ͨ������ϵͳ�ͷŶ�Ӧ����ļ���
                PlayAnim(AnimationName.Anim_Gongji_01);
            }
        }
        else if (dis<= chaseDinstance)
        {
            //��ʾ��ҽ��뵽�����׷�ٷ�Χ���ˣ����ﳯ����ҽ����ƶ�
            if (ActionSate == LogicObjectActionState.Idle|| ActionSate == LogicObjectActionState.Move)
            {
                LogicPos += directionToPlayer * LogicMoveSpeed * LogicFrameConfig.LogicFrameInterval;
                LogicXAxis = directionToPlayer.x;
                PlayAnim(AnimationName.Anim_Walk);
            }
        }
        else
        {
            if (ActionSate == LogicObjectActionState.Idle)
                PlayAnim(AnimationName.Anim_Idle);
        }
    }
    public override void OnHit(string effectHitObjPath, int survivalTimems, LogicObject source,FixInt logicXAxis)
    {
        base.OnHit(effectHitObjPath, survivalTimems, source, logicXAxis);
        this.LogicXAxis = -logicXAxis;
    }

    public override void Floating(bool upfoating)
    {
        base.Floating(upfoating);
        string animName= upfoating ?AnimationName.Anim_Float_up : AnimationName.Anim_Float_down;
        PlayAnim(animName);

        ActionSate = LogicObjectActionState.Floating;
    }

    public override void TriggerGround()
    {
        base.TriggerGround();
        //���������ص��߼�
        if (ObjectState!= LogicObjectState.Death)
        {
            PlayAnim(AnimationName.Anim_Getup);
            //������ӵ�����ȫվ���ʱ����Ҫ���Ŵ�������
            //ͨ���߼�֡�ӳ����ӳ�1�봥���߼�  
            LogicTimerManager.Instance.DelayCall(0.5f,()=> {
                PlayAnim(AnimationName.Anim_Idle);
                ActionSate = LogicObjectActionState.Idle;
            });
        }
        else
        {
            PlayAnim(AnimationName.Anim_Dead);
        }
    }


    private void InitMonsterAttribute()
    {
        MonsterCfg data = ConfigCenter.Instance.GetMonsterCfgById(MonsterId);
        if (data == null)
        {
            Debug.LogError(MonsterId + " MonsterCfg is Null! Return...");
            return;
        }
        hp = data.hp;
        mp = data.mp;
        ap = data.ap;
        ad = data.ad;
        adDef = data.adDef;
        apDef = data.apDef;
        pct = data.pct;
        mct = data.mct;
        adPctRate = data.adPctRate;
        apMctRate = data.apMctRate;
        str = data.str;
        sta = data.sta;
        Int = data.Int;
        spi = data.spi;
        agl = data.agl;

        atkRange = data.atkRange;
        searchDisRange = data.searchDisRange;
        Debug.Log("InitMonsterAttribute Success..");
    }
}
