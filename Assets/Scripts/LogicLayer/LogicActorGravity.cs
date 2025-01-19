using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixMath;
/// <summary>
/// �����߼���������
/// </summary>
public partial class LogicActor
{
    //��ֱ���׹�ʽ v=Vo-gt (V�ٶ� vo����ʼ�ٶ� g����׼���� t��ʱ��)
    protected FixInt gravity = 9.8; //��׼����

    public FixIntVector3 velocity;//��ʼ�ٶ�
    //��ʼ�ٶ�
    private FixInt mVo;
    /// <summary>
    /// ����ʱ��
    /// </summary>
    protected FixInt mRisingTime;

    public bool isAddForce;//�Ƿ����һ����
    /// <summary>
    /// �Ƿ��������
    /// </summary>
    private bool isIgnoreGravity;
    public bool IgnoreGravity { get { return isIgnoreGravity; } set { Debug.Log("isIgnoreGravity:"+value); isIgnoreGravity = value; } }

    /// <summary>
    /// �����߼���������
    /// </summary>
    public void OnLogicFrameUpdateGravity()
    {
        if (isAddForce)
        {
            //1.�����һ�µ��߼���Ҳ���������������һ���߼����������������õ�һ��ʱ���ڡ�
            //1.�ϸ�ʱ�� 2.�¸�ʱ�� 

            //velocity.y ��ʾy���һ���˶��ٶ�// 6   0.64  1��=1֡��1֡=0.066�� c*0.066=t t��������ϸ�����Ҫ��ʱ�� t
            // 6/0.64=c c*L=t  (vo/gt)*L=t t=�������½������ĵ�ʱ�� t*2=����+�½�����Ҫ��ʱ�� 
            //��֪�����������ʱ�䣬��������߼��ڹ涨��ʱ�������� x n 2 0.2f 2*2/0.2
            //(vo/gt)*L=t (t*2)/risingtime=tRate 
            float logicFrameInterval= LogicFrameConfig.LogicFrameInterval;
            FixInt gt = gravity * logicFrameInterval;
            //�����������½�����Ҫ��ʱ��
            FixInt risingForceTime=(mVo / gt) * logicFrameInterval;
            //��ȡʱ�����ű���
            FixInt timeScale= (risingForceTime * 2) / mRisingTime;  

            velocity.y -= (gravity * logicFrameInterval) * timeScale;

            //����Ҫ�ƶ����µ�λ��
            FixIntVector3 newPos = new FixIntVector3(LogicPos.x, FixMath.FixIntMath.Clamp(LogicPos.y + velocity.y * logicFrameInterval, 0, FixInt.MaxValue), LogicPos.z);
            //����Ѿ��������������Ͳ���������λ�ø���
            if (!IgnoreGravity)
            {
                LogicPos = newPos;
            }
      
            //��ʾ���������
            if (newPos.y <= 0)
            {
                Debug.Log("EndTiem:" + Time.realtimeSinceStartup);
                isAddForce = false;
                TriggerGround();
             }
            else
            {
                //�ж϶����Ƿ��������׶�
                if (velocity.y>=0)
                {
                    Floating(true);
                }
                else
                {
                    Floating(false);
                }
            }
        
        }
    }
     /// <summary>
    /// ���������
    /// </summary>
    /// <param name="risingForceValue">��������ֵ</param>
    /// <param name="risingTime">����ʱ��</param>
    public void AddRisingForce(FixInt risingForceValue,FixInt risingTime)
    {
        Debug.Log("StartTiem:"+Time.realtimeSinceStartup);
        mVo=velocity.y = risingForceValue;
        mRisingTime = risingTime*1.0f/1000;
        isAddForce = true;
     }
}
