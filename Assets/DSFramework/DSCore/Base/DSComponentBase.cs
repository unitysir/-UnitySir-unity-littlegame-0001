using System;
using System.Collections;
using System.Collections.Generic;
using DSFramework;
using UnityEngine;
using UnityEngine.UI;

namespace DSFramework
{
    public class DSComponentBase : MonoBehaviour
    {
        #region FindComponent

        /// <summary>
        /// 组件缓存
        /// </summary>
        protected Dictionary<string, List<Component>> CmptDic { get; set; }

        public virtual void InitCmpts()
        {
            CmptDic = new Dictionary<string, List<Component>>();
            DSFindComponentsInChildren<Image>();
            DSFindComponentsInChildren<Button>();
            DSFindComponentsInChildren<InputField>();
            DSFindComponentsInChildren<Text>();
            DSFindComponentsInChildren<Toggle>();
            DSFindComponentsInChildren<RawImage>();
            DSFindComponentsInChildren<Slider>();
            DSFindComponentsInChildren<Dropdown>();
            DSFindComponentsInChildren<Scrollbar>();
            DSFindComponentsInChildren<Animation>();
            DSFindComponentsInChildren<Animator>();
            DSFindComponentsInChildren<AudioSource>();
            DSFindComponentsInChildren<Transform>();
            DSFindComponentsInChildren<CharacterController>();
        }

        /// <summary>
        /// 查找子物体的组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected void DSFindComponentsInChildren<T>() where T : Component
        {
            T[] controls = GetComponentsInChildren<T>(true);
            string objName = "";
            foreach (var t in controls)
            {
                objName = t.gameObject.name;
                if (CmptDic.ContainsKey(objName))
                {
                    CmptDic[objName].Add(t);
                }
                else
                {
                    CmptDic.Add(t.gameObject.name, new List<Component> {t});
                }
            }
        }

        /// <summary>
        /// 通过名称获取组件
        /// </summary>
        /// <param name="objName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T DSCmpt<T>(string objName) where T : Component
        {
            try
            {
                int len = CmptDic[objName].Count; //获取到组件列表的长度
                for (int i = 0; i < len; i++)
                {
                    Component cmpt = CmptDic[objName][i];
                    if (cmpt is T cpt)
                    {
                        return cpt;
                    }
                }
            }
            catch
            {
                throw new Exception($"没有找到该组件:{objName},请确定名称是否正确!");
            }

            return null;
        }

        #endregion

        public virtual void ShutDown()
        {
        }
    }
}