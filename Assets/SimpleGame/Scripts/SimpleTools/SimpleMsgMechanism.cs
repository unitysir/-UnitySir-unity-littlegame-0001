using System;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMsgMechanism
{
    private static Dictionary<string, Action<object[]>> msgDic = new Dictionary<string, Action<object[]>>();

    public static void ReceiveMsg(string msgName, Action<object[]> onMsg)
    {
        if (!msgDic.ContainsKey(msgName))
        {
            msgDic.Add(msgName, onMsg);
        }
    }

    public static void SendMsg(string msgName, params object[] data)
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