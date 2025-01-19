using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    /// <summary>
    /// ����Ŀ��
    /// </summary>
    public Transform target;
    /// <summary>
    /// ƽ��ʱ��
    /// </summary>
    public float smoothTime = 0.3f;
    /// <summary>
    /// ��С�ƶ�λ��(ͨ����ȡ��ͼ���ý��и�ֵ��������Դ������ͨ���ֶ�������ʱ��ֵ)
    /// </summary>
    public Vector2 minPosition;
    /// <summary>
    /// ����ƶ�λ��(ͨ����ȡ��ͼ���ý��и�ֵ��������Դ������ͨ���ֶ�������ʱ��ֵ)
    /// </summary>
    public Vector2 maxPosition;
    /// <summary>
    /// ����ƫ����Ļ����һ����Χ��ʼ����
    /// </summary>
    public float followDistance;

    //1.�ܹ���֤��Ⱦ��ȫ��ɺ���и��� 2.��֤��ɫλ����ȫ���º���и��� 3.�ܹ���ֹ���������
    private void LateUpdate()
    {
        if (target!=null)
        {
            //����Ŀ��λ�ã�ʹĿ��λ�ú������λ�ñ�����ͬһ��z�ᣬ���������������Ҫ�ƶ���x���ֵ
            Vector3 targetPos = new Vector3(target.position.x, transform.position.y, transform.position.z);
            //����������ͽ�ɫ֮��ľ���
            float distance= Vector2.Distance(targetPos,transform.position);
            //�ж�������Ƿ���Ҫ����
            bool isFollowTarget = distance > followDistance;
            //���Ƹ���Ŀ���xλ�ã����ܳ�����ͼ��Χ
            targetPos.x = Mathf.Clamp(targetPos.x,minPosition.x,maxPosition.x);
            if (isFollowTarget)
            {
                transform.position = Vector3.Lerp(transform.position,targetPos, smoothTime*Time.deltaTime);
            }
        }
    }
}
