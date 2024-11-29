using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZMGC.Hall;
public class HallWorldScriptExecutionOrder  :IBehaviourExecution
{
    private static Type[] LogicBehaviorExecutions = new Type[] {
       
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
