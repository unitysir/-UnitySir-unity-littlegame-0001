using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DSFramework {

    /// <summary>
    /// AB 包管理器
    /// </summary>
    public class DSABMgr : DSingleMono<DSABMgr> {
        #region Definition

        /// <summary>
        /// 主包
        /// </summary>
        private AssetBundle _abMain = null;

        /// <summary>
        /// 获取依赖包使用的配置文件
        /// </summary>
        private AssetBundleManifest _abManifest = null;

        /// <summary>
        ///  AB 包不能重复加载,需要使用容器来存储
        /// </summary>
        private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

        /// <summary>
        /// AB包存放路径
        /// </summary>
        private string ABPath => Application.streamingAssetsPath + "/";

        /// <summary>
        /// 根据不同平台获取不同主包名称
        /// </summary>
        private string ABMainName {
            get {
#if UNITY_IOS
                return "IOS";
#elif UNITY_ANDROID
                return "Android";
#else
                return "PC";
#endif
            }
        }

        #endregion

        #region 加载AB包

        /// <summary>
        /// 加载AB包
        /// </summary>
        /// <param name="abName"></param>
        public void LoadAB(string abName) {
            //加载AB包
            if (_abMain == null) {
                _abMain = AssetBundle.LoadFromFile(ABPath + ABMainName);
                //获取固定文件
                _abManifest = _abMain.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
            //获取依赖包相关信息
            AssetBundle ab = null;
            string[] strs = _abManifest.GetAllDependencies(abName);
            for (int i = 0; i < strs.Length; i ++) {
                //判断资源是否加载过
                if (! abDic.ContainsKey(strs[i])) {
                    //如果不存在这个包则加载
                    ab = AssetBundle.LoadFromFile(ABPath + strs[i]);
                    abDic.Add(strs[i], ab);
                }
            }
            // 加载资源的来源包
            if (! abDic.ContainsKey(abName)) {
                ab = AssetBundle.LoadFromFile(ABPath + abName);
                abDic.Add(abName, ab);
            }
        }

        /// <summary>
        /// 异步加载AB包(未完成)
        /// </summary>
        /// <param name="abName"></param>
        public void LoadABAsync(string abName) { }

        #endregion

        #region 资源同步加载

        /// <summary>
        /// 加载AB包资源
        /// </summary>
        /// <param name="abName">包名称</param>
        /// <param name="resName">资源名称</param>
        public Object LoadRes(string abName, string resName) {
            LoadAB(abName);

            // 加载资源
            Object obj = abDic[abName].LoadAsset(resName);
            if (obj is GameObject) {
                return Instantiate(obj);
            } else {
                return obj;
            }
        }

        /// <summary>
        /// 根据类型加载AB包资源
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Object LoadRes(string abName, string resName, System.Type type) {
            LoadAB(abName);
            // 加载资源
            Object obj = abDic[abName].LoadAsset(resName, type);
            if (obj is GameObject) {
                return Instantiate(obj);
            } else {
                return obj;
            }
        }

        /// <summary>
        /// 加载AB包资源 根据泛型类型
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T LoadRes<T>(string abName, string resName) where T : Object {
            LoadAB(abName);

            // 加载资源
            T obj = abDic[abName].LoadAsset<T>(resName);
            if (obj is GameObject) {
                return Instantiate(obj);
            } else {
                return obj;
            }
        }

        #endregion

        #region 异步加载

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        public void LoadResAsync(string abName, string resName, UnityAction<Object> callBack) {
            StartCoroutine(ReallyLoadABAsync(abName, resName, callBack));
        }

        private IEnumerator ReallyLoadABAsync(string abName, string resName, UnityAction<Object> callBack) {
            LoadAB(abName);

            // 加载资源
            AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);

            yield return abr;

            // 异步加载完成后 利用委托把值传出去
            if (abr.asset is GameObject) {
                callBack(Instantiate(abr.asset));
            } else {
                callBack(abr.asset);
            }
        }

        /// <summary>
        /// 根据类型异步加载资源
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        public void LoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack) {
            StartCoroutine(ReallyLoadABAsync(abName, resName, type, callBack));
        }

        private IEnumerator ReallyLoadABAsync(
            string abName,
            string resName,
            System.Type type,
            UnityAction<Object> callBack) {
            LoadAB(abName);

            // 加载资源
            AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);

            yield return abr;

            // 异步加载完成后 利用委托把值传出去
            if (abr.asset is GameObject) {
                callBack(Instantiate(abr.asset));
            } else {
                callBack(abr.asset);
            }
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="resName"></param>
        public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object {
            StartCoroutine(ReallyLoadABAsync<T>(abName, resName, callBack));
        }

        private IEnumerator ReallyLoadABAsync<T>(string abName, string resName, UnityAction<T> callBack)
            where T : Object {
            LoadAB(abName);

            // 加载资源
            AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);

            yield return abr;

            // 异步加载完成后 利用委托把值传出去
            if (abr.asset is GameObject) {
                callBack(Instantiate(abr.asset) as T);
            } else {
                callBack(abr.asset as T);
            }
        }

        #endregion

        #region 卸载单个包

        /// <summary>
        /// 卸载单个包
        /// </summary>
        /// <param name="name"></param>
        /// <param name="unload"></param>
        public void UnLoad(string name, bool unload) {
            AssetBundle ab = null;
            abDic.TryGetValue(name, out ab);
            if (ab != null)
                ab.Unload(unload);
        }

        #endregion

        #region 卸载所有包

        /// <summary>
        /// 卸载所有包
        /// </summary>
        /// <param name="unload"></param>
        public void UnLoadAll(bool unload) {
            AssetBundle.UnloadAllAssetBundles(unload);
            abDic.Clear();
            abDic = null;
            _abMain = null;
            _abManifest = null;
        }

        #endregion
    }

}