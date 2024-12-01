using FixIntPhysics;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillDamageConfig 
{
    [LabelText("触发帧")]
    public int triggerFrame; //触发帧
    [LabelText("结束帧")]
    public int endFrame; //结束帧
    [LabelText("触发间隔ms(0为触发一次)")]
    public int triggerIntervalMs; //触发间隔ms(0为触发一次)
    [LabelText("碰撞体是否跟随特效")]
    public bool isFollowEffect; //碰撞体是否跟随特效
    [LabelText("伤害类型")]
    public DamageType damageType; //伤害类型
    [LabelText("伤害倍率")]
    public int damageRate; //伤害倍率
    [LabelText("伤害检测模式"),OnValueChanged("OnDetectionModeValueChange")]
    public DamageDetectionMode detectionMode; //伤害检测模式
    [LabelText("Box碰撞体的大小"),ShowIf("isShowBox3D")]
    public Vector3 boxSize = new Vector3(1, 1, 1); //Box碰撞体的大小
    [LabelText("Box碰撞体偏移"), ShowIf("isShowBox3D")]
    public Vector3 boxOffset = new Vector3(0, 0, 0); //Box碰撞体偏移
    [LabelText("球型碰撞体偏移"), ShowIf("isShowSphere3D")]
    public Vector3 sphereOffset = new Vector3(0, 0.9f, 0); //球型碰撞体偏移
    [LabelText("圆球伤害检测半径"), ShowIf("isShowSphere3D")]
    public float radius = 1; //圆球伤害检测半径
    [LabelText("圆球伤害检测半径高度"), ShowIf("isShowSphere3D")]
    public float raduisHeight = 0f; //圆球伤害检测半径高度
    [LabelText("碰撞体位置类型")]
    public ColliderPosType colliderPosType = ColliderPosType.FollowDir; //碰撞体位置类型
    [LabelText("伤害触发对象")]
    public TargetType targetType; //伤害触发对象
    [TitleGroup("技能附加Buff","伤害生效的一瞬间，附加指定多个buff")]
    public int[] addBuffs; //技能附加Buff
    [TitleGroup("触发后续技能id","造成伤害后且技能释放完毕后触发的技能")]
    public int triggerSkillid; //触发技能id

    private bool isShowBox3D = false;
    private bool isShowSphere3D = false;
    private FixIntBoxCollider boxCollider;
    private FixIntSphereCollider sphereCollider;

    public void OnDetectionModeValueChange(DamageDetectionMode mode)
    {
        isShowBox3D = mode == DamageDetectionMode.BOX3D;
        isShowSphere3D = mode == DamageDetectionMode.Sphere3D;
        CreateCollider();
    }

    /// <summary>
    /// 获取碰撞体偏移后的位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetColliderOffsetPos()
    {
        Vector3 characterPos = SkillComplierWindow.GetCharacterPos();
        if(detectionMode == DamageDetectionMode.BOX3D)
        {
            return characterPos + boxOffset;
        }
        else if(detectionMode == DamageDetectionMode.Sphere3D)
        {
            return characterPos + sphereOffset;
        }
        return Vector3.zero;
    }


    /// <summary>
    /// 创建碰撞体
    /// </summary>
    public void CreateCollider()
    {
        DestroyCollider();
        if(detectionMode == DamageDetectionMode.BOX3D)
        {
            boxCollider = new FixIntBoxCollider(boxSize, GetColliderOffsetPos());
            boxCollider.SetBoxData(GetColliderOffsetPos(), boxSize, colliderPosType == ColliderPosType.FollowPos);
        }
        else if(detectionMode== DamageDetectionMode.Sphere3D)
        {
            sphereCollider = new FixIntSphereCollider(radius, GetColliderOffsetPos());
            sphereCollider.SetBoxData(radius, GetColliderOffsetPos(), colliderPosType == ColliderPosType.FollowPos);
        }


    }

    public void DestroyCollider()
    {
        if(boxCollider!= null)
        {
            boxCollider.OnRelease();
        }
        if(sphereCollider!= null)
        {
            sphereCollider.OnRelease();
        }
    }


}


public enum ColliderPosType
{
    [LabelText("跟随角色方向")]FollowDir,//跟随角色方向
    [LabelText("跟随角色位置")] FollowPos,//跟随角色位置
    [LabelText("中心坐标")] CenterPos,//中心坐标
    [LabelText("目标位置")] TargetPos,//目标位置
}


public enum TargetType
{
    [LabelText("未配置")] None,//未配置
    [LabelText("友军")] Teammate,//友军
    [LabelText("敌军")] Enemy,//敌军
    [LabelText("自己")] Self,//自己
    [LabelText("所有对象")] AllObject,//所有对象
}


public enum DamageType
{
    [LabelText("无配置")] None,//无配置
    [LabelText("魔法伤害")] APDamage,//魔法伤害
    [LabelText("物理伤害")] ADDamage,//物理伤害
}


public enum DamageDetectionMode
{
    [LabelText("无配置")] None,//无配置
    [LabelText("3DBox碰撞检测")] BOX3D,//3DBox碰撞检测
    [LabelText("3D球型检测")] Sphere3D,//3D球型检测
    [LabelText("3D圆柱检测")] Cylinder3d,//3D圆柱检测
    [LabelText("半径的距离")] RadiusDistance,//半径的距离
    [LabelText("所有目标}")] AllTarget,//通过代码搜索所有目标
}






