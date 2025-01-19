using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[HideMonoScript]
[System.Serializable]
public class SkillCharacterConfig 
{
    [AssetList]
    [LabelText("��ɫģ��")]
    [PreviewField(70,ObjectFieldAlignment.Center)]
    public GameObject skillChararcter;
    [LabelText("���ܶ���")]
    [TitleGroup("������Ⱦ","����Ӣ����Ⱦ���ݻ��ڼ��ܿ�ʼ�ͷ�ʱ����")]
    public AnimationClip skillAnim;

    [BoxGroup("��������")][ProgressBar(0,100,r:0,g:255,b:0,Height =30)][HideLabel][OnValueChanged("OnAnimProgressValueChange")]
    public short animProgress = 0;
    [BoxGroup("��������")]
    [LabelText("�Ƿ�ѭ������")]
    public bool isLoopAnim = false;
    [LabelText("����ѭ������")][ShowIf("isLoopAnim")]
    [BoxGroup("��������")]
    public int animLoopCount;
    [LabelText("�߼�֡��")]
    [BoxGroup("��������"),HideIf("isSetCustomLogicFrame")]
    public int logicFrame = 0;
    [LabelText("�Ƿ������Զ����߼�֡��")]
    [BoxGroup("��������")]
    public bool isSetCustomLogicFrame=false;
    [LabelText("�Զ����߼�֡��")]
    [BoxGroup("��������"),ShowIf("isSetCustomLogicFrame")]
    public int customLogicFame = 0;
    [LabelText("��������")]
    [BoxGroup("��������")]
    public float animLength = 0;
    [LabelText("�����Ƽ�ʱ��(����ms)")]
    [BoxGroup("��������")]
    public float skillDurationMS = 0;


    private GameObject mTempChararcter;
    private bool mIsPlayAnim=false;//�Ƿ񲥷Ŷ���������������ͣ����
    private double mLastRunTime = 0;//�ϴ����е�ʱ��
    private Animation mAnimtion = null;
 
    [GUIColor(0.4f, 0.8f, 1)]
    [ButtonGroup("��ť����")]
    [Button("����",  ButtonSizes.Large)]
    public void Play()
    {
        if (skillChararcter!=null)
        {
            //�ȴӳ����в��Ҽ��ܶ���������Ҳ�������������¡һ��
            string charactorName= skillChararcter.name;
            mTempChararcter= GameObject.Find(charactorName);
            if (mTempChararcter == null)
            {
                mTempChararcter = GameObject.Instantiate(skillChararcter);
                mTempChararcter.name= mTempChararcter.name.Replace("(Clone)","");
            }
            //�ж�ģ�������Ƿ��иö��������û����������
            mAnimtion = mTempChararcter.GetComponent<Animation>();
            if (!mAnimtion.GetClip(skillAnim.name))
            {
                mAnimtion.AddClip(skillAnim, skillAnim.name);
            }
            mAnimtion.clip = skillAnim;
            //���㶯���ļ�����
            animLength = isLoopAnim ? skillAnim.length * animLoopCount : skillAnim.length;
            //�����߼�֡���ȣ�������
            logicFrame = (int)(isLoopAnim ? skillAnim.length / 0.066f * animLoopCount : skillAnim.length / 0.066f);
            //���㼼���Ƽ�ʱ��
            skillDurationMS = (int)(isLoopAnim ? (skillAnim.length * animLoopCount) * 1000 : skillAnim.length * 1000);
            mLastRunTime = 0;
            //��ʼ���Ž�ɫ����
            mIsPlayAnim = true;
            SkillComplierWindow window= SkillComplierWindow.GetWindow();
            window?.StartPlaySkill();
        }
    }
    [ButtonGroup("��ť����")]
    [Button("��ͣ", ButtonSizes.Large)]
    public void Pause()
    {
        mIsPlayAnim = false;
        SkillComplierWindow window = SkillComplierWindow.GetWindow();
        window?.SkillPause();
    }
    [GUIColor(0, 1, 0)]
    [ButtonGroup("��ť����")]
    [Button("��������", ButtonSizes.Large)]
    public void SaveAssets()
    {
        SkillComplierWindow.GetWindow().SaveSKillData();
    }

    public void OnUpdate(System.Action progressUpdateCallback)
    {
        if (mIsPlayAnim)
        {
            if (mLastRunTime==0)
            {
                mLastRunTime = EditorApplication.timeSinceStartup;
            }
            //��ȡ��ǰ���е�ʱ��
            double curRunTime = EditorApplication.timeSinceStartup - mLastRunTime;

            //���㶯�����Ž���
            float curAnimNormalizationValue = (float)curRunTime / animLength;
            animProgress = (short)Mathf.Clamp(curAnimNormalizationValue*100,0,100);
            //�����߼�֡
            logicFrame = (int)(curRunTime / LogicFrameConfig.LogicFrameInterval);
            //�������������ж�������
            mAnimtion.clip.SampleAnimation(mTempChararcter,(float)curRunTime);

            if (animProgress==100)
            {
                //�����������
                PlaySkillEnd();
            }
            //�������ھ۽��ص���ˢ�´���
            progressUpdateCallback?.Invoke();
        }
    }
    /// <summary>
    /// �������ȸı����
    /// </summary>
    /// <param name="value"></param>
    public void OnAnimProgressValueChange(float value)
    {
        //�ȴӳ����в��Ҽ��ܶ���������Ҳ�������������¡һ��
        string charactorName = skillChararcter.name;
        mTempChararcter = GameObject.Find(charactorName);
        if (mTempChararcter == null)
        {
            mTempChararcter = GameObject.Instantiate(skillChararcter);
            mTempChararcter.name = mTempChararcter.name.Replace("(Clone)", "");
        }
        //�ж�ģ�������Ƿ��иö��������û����������
        mAnimtion = mTempChararcter.GetComponent<Animation>();
        //���ݵ�ǰ�������Ƚ��ж�������
        float progressValue = (value / 100) * skillAnim.length;
        logicFrame =(int) (progressValue / LogicFrameConfig.LogicFrameInterval);
        //�������������ж�������
        mAnimtion.clip.SampleAnimation(mTempChararcter, progressValue);

    }
    public void PlaySkillEnd()
    {
        mIsPlayAnim = false;

        SkillComplierWindow window= SkillComplierWindow.GetWindow();
        window?.PlaySkilEnd();
    }
}
