using System;
using System.Collections;
using UnityEngine;

namespace DSFramework
{
    public class MonoComponent : DSComponent
    {
        private DSMonoController controller;

        public override void InitCmpts()
        {
            base.InitCmpts();
            controller = GetComponent<DSMonoController>();
        }

        #region Update

        public void AddUpdate(string funName, Action fun)
        {
            controller.AddUpdate(funName, fun);
        }

        public void DelUpdate(string funName)
        {
            //controller.DelUpdate(funName);
        }

        #endregion

        #region LateUpdate

        public void AddLateUpdate(string funName, Action fun)
        {
            controller.AddLateUpdate(funName, fun);
        }

        public void DelLateUpdate(string funName)
        {
            controller.DelLateUpdate(funName);
        }

        #endregion

        #region FixedUpdate

        public void AddFixedUpdate(string funName, Action fun)
        {
            controller.AddFixedUpdate(funName, fun);
        }

        public void DelFixedUpdate(string funName)
        {
            controller.DelFixedUpdate(funName);
        }

        #endregion

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return controller.StartCoroutine(routine);
        }
    }
}