using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZMGC.Battle;
public class BattleWorldScriptExecutionOrder : IBehaviourExecution
{
    private static Type[] LogicBehaviorExecutions = new Type[] {

        typeof(HeroLogicCtrl), 
        typeof(MonsterLogicCtrl)
     };

    private static Type[] DataBehaviorExecutions = new Type[] {

     };

    private static Type[] MsgBehaviorExecutions = new Type[] {

     };

    public Type[] GetDataBehaviourExecution()
    {
        return DataBehaviorExecutions;
    }

    public Type[] GetLogicBehaviourExecution()
    {
        return LogicBehaviorExecutions;
    }

    public Type[] GetMsgBehaviourExecution()
    {
        return MsgBehaviorExecutions;
    }
}
