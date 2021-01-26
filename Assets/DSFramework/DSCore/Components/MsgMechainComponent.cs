using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DSFramework
{
    public interface IEventInfo
    {
    }

    public class EventInfo<T> : IEventInfo
    {
        public UnityAction<T> actions;

        public EventInfo(UnityAction<T> action)
        {
            actions += action;
        }
    }

    public class EventInfo : IEventInfo
    {
        public UnityAction actions;

        public EventInfo(UnityAction action)
        {
            actions += action;
        }
    }

    public class MsgMechainComponent : DSComponent
    {
        
        private Dictionary<string, IEventInfo> eventDic;

        public override void InitCmpts()
        {
            base.InitCmpts();
            eventDic = new Dictionary<string, IEventInfo>();
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="action">准备用来处理事件 的委托函数</param>
        public void Receiver<T>(string name, UnityAction<T> action)
        {
            //有没有对应的事件监听
            //有的情况
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo<T>).actions += action;
            }
            //没有的情况
            else
            {
                eventDic.Add(name, new EventInfo<T>(action));
            }
        }

        /// <summary>
        /// 监听不需要参数传递的事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public void Receiver(string name, UnityAction action)
        {
            //有没有对应的事件监听
            //有的情况
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo).actions += action;
            }
            //没有的情况
            else
            {
                eventDic.Add(name, new EventInfo(action));
            }
        }


        /// <summary>
        /// 移除对应的事件监听
        /// </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="action">对应之前添加的委托函数</param>
        public void Remove<T>(string name, UnityAction<T> action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo<T>).actions -= action;
        }

        /// <summary>
        /// 移除不需要参数的事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public void Remove(string name, UnityAction action)
        {
            if (eventDic.ContainsKey(name))
                (eventDic[name] as EventInfo).actions -= action;
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="name">哪一个名字的事件触发了</param>
        public void Sender<T>(string name, T info)
        {
            //有没有对应的事件监听
            //有的情况
            if (eventDic.ContainsKey(name))
            {
                //eventDic[name]();
                if ((eventDic[name] as EventInfo<T>).actions != null)
                    (eventDic[name] as EventInfo<T>).actions.Invoke(info);
                //eventDic[name].Invoke(info);
            }
        }

        /// <summary>
        /// 事件触发（不需要参数的）
        /// </summary>
        /// <param name="name"></param>
        public void Sender(string name)
        {
            //有没有对应的事件监听
            //有的情况
            if (eventDic.ContainsKey(name))
            {
                //eventDic[name]();
                if ((eventDic[name] as EventInfo).actions != null)
                    (eventDic[name] as EventInfo).actions.Invoke();
                //eventDic[name].Invoke(info);
            }
        }

        /// <summary>
        /// 清空事件中心
        /// 主要用在 场景切换时
        /// </summary>
        public void Clear()
        {
            eventDic.Clear();
        }

        public override void ShutDown()
        {
            base.ShutDown();
            Clear();
            eventDic = null;
        }
    }
}