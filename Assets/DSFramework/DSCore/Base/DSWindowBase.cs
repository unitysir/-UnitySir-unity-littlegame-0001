using System.Collections.Generic;
using DSFramework;

namespace DSFramework
{
    public class DSWindowBase : DSComponentBase
    {
        /// <summary>
        /// 是否初始化
        /// </summary>
        public virtual bool isInit { get; set; } = false;

        /// <summary>
        /// 窗体缓存列表
        /// </summary>
        protected static List<DSWindowBase> windowList = new List<DSWindowBase>();

        /// <summary>
        /// 窗体预设路径
        /// </summary>
        ///public virtual string PrefabPath { get; private set; }

        public override void InitCmpts()
        {
            base.InitCmpts();
            windowList.Add(this);
        }
    }
}