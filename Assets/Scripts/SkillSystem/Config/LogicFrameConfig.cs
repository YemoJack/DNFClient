using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicFrameConfig : MonoBehaviour
{
    //逻辑帧ID 自增
    public static long LogicFrameid;
    //实际逻辑帧间隔
    public static float LogicFrameInterval = 0.066f; //一秒十五帧
    //毫秒级逻辑帧间隔 用来计算当前逻辑帧累加时间
    public static int LogicFrameIntervalMS = 66;
}
