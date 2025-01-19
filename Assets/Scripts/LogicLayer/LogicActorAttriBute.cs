using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LogicActor
{
    protected FixInt level = 1;//�ȼ�
    protected string name;//����
    protected FixInt id;//Ψһid
    protected FixInt type;//����

    #region �ڲ�����(�����������)
    protected FixInt hp;//Ѫ��
    protected FixInt mp;//����ֵ
    protected FixInt ap;//ħ��������
    protected FixInt ad;//��������
    protected FixInt adDef;//���������
    protected FixInt apDef;//ħ��������
    protected FixInt pct;//��������
    protected FixInt mct;//ħ��������
    protected FixInt adPctRate;//����������
    protected FixInt apMctRate;//ħ����������
    //��ά
    protected FixInt str;//����
    protected FixInt sta;//����
    protected FixInt Int;//����
    protected FixInt spi;//����

    protected FixInt agl;//����

    protected FixInt atkRange; //�������룬��������Զ�̹���ͽ�ս����Ĺ�������
    protected FixInt searchDisRange;//��Ѱ���� ���ڳ������״���ѰĿ����н���׷��
    #endregion

    #region ս��ʱͨ��buff���ӵ�����
    public FixInt addADDef;//ս��ʱͨ��buff���ӵķ�����
    public FixInt addAPDef;
    public FixInt addAD;
    public FixInt addAP;
    public FixInt addMCT;
    public int addPCT;
    public FixInt addAPMACTRate;
    public FixInt addAdPCTRate;

    public FixInt addStr;//����
    public FixInt addSta;//����
    public FixInt addInt;//����
    public FixInt addSpi;//����

    public FixInt addAgl;//����
    #endregion

    #region ��������
    public FixInt HP { get { return hp ; }}//Ѫ��
    public FixInt MP { get { return mp; } }//����ֵ
    public FixInt AP { get { return addAP + ap; } }//ħ��������
    public FixInt AD { get { return addAD + ad; } }//��������
    public FixInt ADDef { get { return addADDef + adDef; } }//���������
    public FixInt APdef { get { return addAPDef + apDef; } }//ħ��������
    public FixInt PCT { get { return addPCT + pct; } }//��������
    public FixInt MCT { get { return addMCT + mct; } }//ħ��������
    public FixInt ADPCTRate { get { return addAdPCTRate + adPctRate; } }//����������
    public FixInt APMCTRate { get { return addAPMACTRate + apMctRate; } }//ħ����������
    //��ά
    public FixInt STR { get { return addStr + str; } }//����
    public FixInt STA { get { return addSta + sta; } }//����
    public FixInt INT { get { return addInt + Int; } }//����
    public FixInt SPI { get { return addSpi + spi; } }//����

    public FixInt AGL { get { return addAgl + agl; } }//����

    public FixInt Level { get { return level; } }//�ȼ�
    #endregion
    /// <summary>
    /// ����Ѫ��
    /// </summary>
    /// <param name="reduceHp"></param>
    public void ReduceHP(FixInt reduceHp)
    {
        hp -= reduceHp;
        if (hp<=0)
        {
            hp = 0;
        }
    }
}
