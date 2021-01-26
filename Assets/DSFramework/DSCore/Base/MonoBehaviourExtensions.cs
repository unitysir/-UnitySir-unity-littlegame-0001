using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DSFramework
{
    public static class MonoBehaviourExtensions
    {
        #region Transform

        /// <summary>
        /// 通过变换获取儿子的组件
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="transName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T DSGetChildCmpt4Trans<T>(this Transform trans, string transName)
        {
            return trans.Find(transName).GetComponent<T>();
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region GameObject

        public static T DSGetCmpt4Tag<T>(this GameObject obj, string tagName)
        {
            return GameObject.FindWithTag(tagName).GetComponent<T>();
        }

        public static T DSGetCmpt4Name<T>(this GameObject obj, string objName)
        {
            return GameObject.Find(objName).GetComponent<T>();
        }

        public static void DSetLocalPosX(this GameObject obj, float x)
        {
            var localPosition = obj.transform.localPosition;
            localPosition = new Vector3(x, localPosition.y, localPosition.z);
            obj.transform.localPosition = localPosition;
        }

        public static void DSetLocalPosY(this GameObject obj, float y)
        {
            var localPosition = obj.transform.localPosition;
            localPosition = new Vector3(localPosition.x, y, localPosition.z);
            obj.transform.localPosition = localPosition;
        }

        public static void DSetLocalPosZ(this GameObject obj, float z)
        {
            var localPosition = obj.transform.localPosition;
            localPosition = new Vector3(localPosition.x, localPosition.y, z);
            obj.transform.localPosition = localPosition;
        }
        
        public static void DSetPosX(this GameObject obj, float x)
        {
            var position = obj.transform.position;
            position = new Vector3(x, position.y, position.z);
            obj.transform.localPosition = position;
        }

        public static void DSetPosY(this GameObject obj, float y)
        {
            var position = obj.transform.position;
            position = new Vector3(position.x, y, position.z);
            obj.transform.localPosition = position;
        }

        public static void DSetPosZ(this GameObject obj, float z)
        {
            var position = obj.transform.position;
            position = new Vector3(position.x, position.y, z);
            obj.transform.localPosition = position;
        }

        public static void DSetParent(this GameObject obj, GameObject o)
        {
            obj.transform.SetParent(o.transform, false);
        }

        public static void DSetParent(this GameObject obj, Transform o)
        {
            obj.transform.SetParent(o, false);
        }

        public static void DSReSetLocalPos(this GameObject obj)
        {
            obj.transform.localPosition = Vector3.zero;
        }

        public static void DSReSetLocalScale(this GameObject obj)
        {
            obj.transform.localScale = Vector3.one;
        }

        public static void DShow(this GameObject obj)
        {
            obj.SetActive(true);
        }

        public static void DSHide(this GameObject obj)
        {
            obj.SetActive(false);
        }

        public static T GetOrAddCmpt<T>(this GameObject go) where T : Component
        {
            T t = go.GetComponent<T>();
            if (t == null)
            {
                t = go.AddComponent<T>();
            }

            return t;
        }

        #endregion
    }
}