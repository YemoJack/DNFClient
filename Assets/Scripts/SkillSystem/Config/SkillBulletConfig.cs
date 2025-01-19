using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class SkillBulletConfig  
{
    [AssetList,LabelText("��Ч"),PreviewField(70,ObjectFieldAlignment.Left),OnValueChanged("GetBulletObjectPath")]
    public GameObject bulletPrefab;
    [ReadOnly]
    public string bulletPrefabPath;
    [LabelText("��������Ѱ��(��������ͨ�ӵ������ܵ����ӵ�����Ƕ�����ǰ������)")]
    public bool interlligentAttack;
    [LabelText("����֡")]
    public int triggerFrame;
    [LabelText("�Ƿ�ѭ������"),BoxGroup("ѭ����������")]
    public bool isLoopCreate;
    [LabelText("ѭ�����ms ����"),ShowIf("isLoopCreate"), BoxGroup("ѭ����������")]
    public int loopIntervalMs;
    [LabelText("��С���λ�ò�����Χ"), ShowIf("isLoopCreate"), BoxGroup("ѭ����������")]
    public Vector3 minrandomRangeVect3;
    [LabelText("������λ�ò�����Χ"), ShowIf("isLoopCreate"), BoxGroup("ѭ����������")]
    public Vector3 maxRandomRangeVect3;
    [LabelText("�ƶ��ٶ�")]
    public float moveSpeed;
    [LabelText("���ʱ�䣨���룩")]
    public int survivalTimeMsg;
    [LabelText("�������ٶ�")]
    public Vector2 gravitySpeed;
    [LabelText("����λ��ƫ��")]
    public Vector3 offset;
    [LabelText("���䷽��")]
    public Vector3 dir;
    [LabelText("����Ƕ�ƫ��")]
    public Vector3 angle;
    [LabelText("�Ƿ��������")]
    public bool isHitDestory = true;
    [LabelText("������Ч"),PreviewField(70,ObjectFieldAlignment.Left),OnValueChanged("GetHitEffectObjectPath")]
    public GameObject hitEffect;
    [ReadOnly]
    public string hitEffectPath;
    [LabelText("������Ч���ʱ��")]
    public int hitEffectSurvivalTimems = 3000;
    [LabelText("������Ч")]
    public AudioClip hitAudio;
    [ToggleGroup("isAttachDamage","�Ƿ񸽼��˺�")]
    public bool isAttachDamage = false;
    [ToggleGroup("isAttachDamage", "�Ƿ񸽼��˺�")]
    public SkillDamageConfig damageCfg;
#if UNITY_EDITOR
    public void GetBulletObjectPath(GameObject obj)
    {
        bulletPrefabPath = UnityEditor.AssetDatabase.GetAssetPath(obj);
        Debug.Log("GetBulletObjectPath:" + bulletPrefabPath);
    }
    public void GetHitEffectObjectPath(GameObject obj)
    {
        hitEffectPath = UnityEditor.AssetDatabase.GetAssetPath(obj);
        Debug.Log("GetHitEffectObjectPath:" + hitEffectPath);
    }
#endif
}
