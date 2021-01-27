using System;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMsgMechanism
{

    private static SimpleMsgMechanism _instance = new SimpleMsgMechanism();

    public static SimpleMsgMechanism Instance => _instance;

    private static Dictionary<string, Action<object>> msgDic = new Dictionary<string, Action<object>>();

    public void ReceiveMsg(string msgName, Action<object> onMsg)
    {
        if (!msgDic.ContainsKey(msgName))
        {
            msgDic.Add(msgName, onMsg);
        }
        else
        {
            Debug.LogError($"消息名称重复：{msgName}");
        }
    }

    public void SendMsg(string msgName, object data)
    {
        if (msgDic.ContainsKey(msgName))
        {
            msgDic[msgName]?.Invoke(data);
        }
    }

    public void ClearAllMsg()
    {
        msgDic.Clear();
    }
}