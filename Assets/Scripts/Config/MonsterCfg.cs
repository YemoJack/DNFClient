using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MonsterType
{ 
    Normal=1,
    Elite=2,
    Boss=5,
}
public class MonsterCfg  
{
    public int id;//����id
    public string name;//��������
    public MonsterType type;//�������� 1.��ͨ���� 2.��Ӣ���� 5.Boss
    public string monsterDes;//��������
    public int[] skillidArr;//��������
    public int hp;//Ѫ��
    public int mp;//����ֵ
    public int ap;//��������
    public int ad;//�������
    public int adDef;//����ֵ
    public int apDef;//����ֵ
    public int pct;//����ֵ
    public int mct;//����ֵ
    public float adPctRate;//����ֵ
    public float apMctRate;//����ֵ
    public int str;//����ֵ
    public int sta;//����ֵ
    public int Int;//����ֵ
    public int spi;//����ֵ
    public int agl;//����ֵ
    public int atkRange; //�������룬��������Զ�̹���ͽ�ս����Ĺ�������
    public int searchDisRange;//��Ѱ���� ���ڳ������״���ѰĿ����н���׷��
}
