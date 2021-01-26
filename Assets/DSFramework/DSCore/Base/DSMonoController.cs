/****************************************************
--------------------------------
    ----------------------------
    文件名称：
    作者：UnitySir
    创建日期：2020年12月05日 08:55:11
    ----------------------------
    ----------------------------
    功能描述：
    ----------------------------
    ----------------------------
--------------------------------
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DSFramework
{
    internal class DSMonoController : MonoBehaviour
    {
    
        #region Test

        private Dictionary<string, Action> UpdateDic = new Dictionary<string, Action>();
        private Dictionary<string, Action> LateUpdateDic = new Dictionary<string, Action>();
        private Dictionary<string, Action> FixedUpdateDic = new Dictionary<string, Action>();

        private GameObject UpdateObj = null;
        private GameObject LateUpdateObj = null;
        private GameObject FixedUpdateObj = null;

        #region Update

        public void AddUpdate(string funName, Action fun)
        {
            if (UpdateObj == null)
            {
                UpdateObj = GameObject.Find("Update");
            }

            try
            {
                UpdateDic.Add(funName, fun);
                GameObject obj = new GameObject(funName);
                obj.DSetParent(UpdateObj);
            }
            catch
            {
                Debug.LogError("Update名称重复");
            }
        }

        public void DelUpdate(string funName)
        {
            UpdateDic.Remove(funName);
            Destroy(GameObject.Find(funName));
        }

        #endregion

        #region LateUpdate

        public void AddLateUpdate(string funName, Action fun)
        {
            if (LateUpdateObj == null)
            {
                LateUpdateObj = GameObject.Find("LateUpdate");
            }

            try
            {
                LateUpdateDic.Add(funName, fun);
                GameObject obj = new GameObject(funName);
                obj.DSetParent(LateUpdateObj);
            }
            catch
            {
                Debug.LogError("LateUpdate名称重复");
            }
        }

        public void DelLateUpdate(string funName)
        {
            LateUpdateDic.Remove(funName);
            Destroy(GameObject.Find(funName));
        }

        #endregion

        #region FixedUpdate

        public void AddFixedUpdate(string funName, Action fun)
        {
            if (FixedUpdateObj == null)
            {
                FixedUpdateObj = GameObject.Find("FixedUpdate");
            }

            try
            {
                FixedUpdateDic.Add(funName, fun);
                GameObject obj = new GameObject(funName);
                obj.DSetParent(FixedUpdateObj);
            }
            catch
            {
                Debug.LogError("FixedUpdate名称重复");
            }
        }

        public void DelFixedUpdate(string funName)
        {
            FixedUpdateDic.Remove(funName);
            Destroy(GameObject.Find(funName));
        }

        #endregion

        private void Update()
        {
            var enumerator = UpdateDic.GetEnumerator();

            try
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current.Value.Invoke();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        private void LateUpdate()
        {
            var enumerator = LateUpdateDic.GetEnumerator();

            try
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current.Value.Invoke();
                }
            }
            catch
            {
                // ignored
            }
        }

        private void FixedUpdate()
        {
            var enumerator = FixedUpdateDic.GetEnumerator();

            try
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current.Value.Invoke();
                }
            }
            catch
            {
                // ignored
            }
        }

        #endregion
    }
}