using UnityEngine;

namespace DSFramework
{
    public class DebugsComponent : DSComponent
    {
        public override void InitCmpts()
        {
            base.InitCmpts();
        }

        public override void ShutDown()
        {
            base.ShutDown();
        }


        [Header("是否打开日志调试")] [SerializeField] private bool openDebugLog = true;

        /// <summary>
        /// 打开调试日志
        /// </summary>
        public bool OpenDebugLog
        {
            get => openDebugLog;
            set => openDebugLog = value;
        }

        //----------------------

        [Header("热更新是否使用AssetBundle模式")] [SerializeField]
        private bool useAB = true;

        /// <summary>
        /// 是否使用Assetbundle模式
        /// </summary>
        public bool UseAB
        {
            get => useAB;
            set => useAB = value;
        }

        //-----------------------

        [Header("是否将AB包拷贝到StreamingAssets")] [SerializeField]
        private bool isCopyABTo = true;

        public bool IsCopyAbTo
        {
            get => isCopyABTo;
            set => isCopyABTo = value;
        }
    }
}