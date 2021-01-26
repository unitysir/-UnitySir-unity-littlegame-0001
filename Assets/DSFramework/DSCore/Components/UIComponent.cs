using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DSFramework
{
    public enum UILayer
    {
        Background,
        Normal,
        Tip,
    }

    public class UIComponent : DSComponent
    {
        private static Dictionary<string, GameObject> windowDic;

        private Transform m_Canvas;
        private Transform m_Background;
        private Transform m_Normal;
        private Transform m_Tip;

        public override void InitCmpts()
        {
            windowDic = new Dictionary<string, GameObject>();

            if (m_Canvas == null)
            {
                m_Canvas = GameObject.FindWithTag("UIRoot").transform;
                m_Background = m_Canvas.Find("Background");
                m_Normal = m_Canvas.Find("Normal");
                m_Tip = m_Canvas.Find("Tip");
            }
        }

        /// <summary>
        ///打开面板(获取到window上的T脚本)
        /// </summary>
        /// <param name="winName">窗体</param>
        /// <typeparam name="T">窗体对应脚本窗体对应脚本</typeparam>
        /// <returns>脚本</returns>
        public T Open<T>(string winName, UILayer uiLayer, bool? isHot = false, bool? isAb = false, string abName = null)
            where T : DSWindowBase
        {
            if (windowDic.TryGetValue(winName, out var window))
            {
                if (!window.activeSelf)
                {
                    window.DShow();
                }

                return GetOrAddCmpt<T>(window);
            }

            if (isAb == true && abName != null)
            {
                window = Get<T>(winName, uiLayer, abName).gameObject;
            }
            else if (isHot == true && abName != null)
            {
                window = GetHot<T>(winName, uiLayer, abName).gameObject;
            }
            else
            {
                window = Get<T>(winName, uiLayer).gameObject;
            }

            if (!window.activeSelf)
            {
                window.DShow();
            }

            return GetOrAddCmpt<T>(window);
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        /// <param name="winName"></param>
        /// <param name="win"></param>
        public void Close(string winName, bool isUn = false)
        {
            if (windowDic.TryGetValue(winName, out var window))
            {
                window = windowDic[winName];
                if (isUn)
                {
                    DSEntity.Resource.UnLoadRes(winName);
                    windowDic.Remove(winName);
                    Object.Destroy(window);
                }
                else
                {
                    if (window.activeSelf)
                    {
                        window.DShow();
                    }
                }
            }
        }

        /// <summary>
        /// 获取窗体
        /// </summary>
        /// <param name="winName"></param>
        /// <typeparam name="T">窗体脚本</typeparam>
        /// <returns>窗体</returns>
        private T Get<T>(string winName, UILayer uiLayer) where T : DSWindowBase
        {
            if (windowDic.TryGetValue(winName, out var window))
            {
                T win = GetOrAddCmpt<T>(window);
                switch (uiLayer)
                {
                    case UILayer.Background:
                        window.DSetParent(m_Background);
                        break;
                    case UILayer.Normal:
                        window.DSetParent(m_Normal);
                        break;
                    case UILayer.Tip:
                        window.DSetParent(m_Tip);
                        break;
                }

                window.transform.localPosition = Vector3.zero;
                window.transform.localScale = Vector3.one;
                window.DSHide();
                return win;
            }

            window = DSEntity.Resource.LoadRes(winName);
            T t = GetOrAddCmpt<T>(window);
            switch (uiLayer)
            {
                case UILayer.Background:
                    window.DSetParent(m_Background);
                    break;
                case UILayer.Normal:
                    window.DSetParent(m_Normal);
                    break;
                case UILayer.Tip:
                    window.DSetParent(m_Tip);
                    break;
            }

            window.transform.localPosition = Vector3.zero;
            window.transform.localScale = Vector3.one;
            if (!t.isInit)
            {
                t.InitCmpts();
                t.isInit = true;
            }

            RectTransform rc = GetOrAddCmpt<RectTransform>(window);
            rc.sizeDelta = Vector2.zero;
            if (!windowDic.ContainsKey(winName))
            {
                windowDic.Add(winName, window);
            }

            window.SetActive(false);

            return GetOrAddCmpt<T>(window);
        }

        private T Get<T>(string winName, UILayer uiLayer, string abName) where T : DSWindowBase
        {
            if (windowDic.TryGetValue(winName, out var window))
            {
                T win = GetOrAddCmpt<T>(window);
                switch (uiLayer)
                {
                    case UILayer.Background:
                        window.DSetParent(m_Background);
                        break;
                    case UILayer.Normal:
                        window.DSetParent(m_Normal);
                        break;
                    case UILayer.Tip:
                        window.DSetParent(m_Tip);
                        break;
                }

                window.transform.localPosition = Vector3.zero;
                window.transform.localScale = Vector3.one;
                window.DSHide();
                return win;
            }

            window = DSEntity.Resource.LoadAbRes(abName, winName) as GameObject;
            // window.name = winName;
            T t = GetOrAddCmpt<T>(window);
            switch (uiLayer)
            {
                case UILayer.Background:
                    window.DSetParent(m_Background);
                    break;
                case UILayer.Normal:
                    window.DSetParent(m_Normal);
                    break;
                case UILayer.Tip:
                    window.DSetParent(m_Tip);
                    break;
            }

            window.transform.localPosition = Vector3.zero;
            window.transform.localScale = Vector3.one;
            if (!t.isInit)
            {
                t.InitCmpts();
                t.isInit = true;
            }

            RectTransform rc = GetOrAddCmpt<RectTransform>(window);
            rc.sizeDelta = Vector2.zero;
            if (!windowDic.ContainsKey(winName))
            {
                windowDic.Add(winName, window);
            }

            window.DSHide();

            return GetOrAddCmpt<T>(window);
        }

        private T GetHot<T>(string winName, UILayer uiLayer, string abName) where T : DSWindowBase
        {
            if (windowDic.TryGetValue(winName, out var window))
            {
                T win = GetOrAddCmpt<T>(window);
                switch (uiLayer)
                {
                    case UILayer.Background:
                        window.DSetParent(m_Background);
                        break;
                    case UILayer.Normal:
                        window.DSetParent(m_Normal);
                        break;
                    case UILayer.Tip:
                        window.DSetParent(m_Tip);
                        break;
                }

                window.transform.localPosition = Vector3.zero;
                window.transform.localScale = Vector3.one;
                window.DSHide();
                return win;
            }

            // TODO 未完成 window = Instantiate(DSEntity.AssetBundle.GetAsset(abName, winName) as GameObject);
            // window.name = winName;
            T t = GetOrAddCmpt<T>(window);
            switch (uiLayer)
            {
                case UILayer.Background:
                    window.DSetParent(m_Background);
                    break;
                case UILayer.Normal:
                    window.DSetParent(m_Normal);
                    break;
                case UILayer.Tip:
                    window.DSetParent(m_Tip);
                    break;
            }

            window.transform.localPosition = Vector3.zero;
            window.transform.localScale = Vector3.one;
            if (!t.isInit)
            {
                t.InitCmpts();
                t.isInit = true;
            }

            RectTransform rc = GetOrAddCmpt<RectTransform>(window);
            rc.sizeDelta = Vector2.zero;
            if (!windowDic.ContainsKey(winName))
            {
                windowDic.Add(winName, window);
            }

            window.DSHide();

            return GetOrAddCmpt<T>(window);
        }

        private T GetOrAddCmpt<T>(GameObject go) where T : Component
        {
            T t = go.GetComponent<T>();
            if (t == null)
            {
                t = go.AddComponent<T>();
            }

            return t;
        }

        public override void ShutDown()
        {
            windowDic.Clear();
            windowDic = null;
        }
    }
}