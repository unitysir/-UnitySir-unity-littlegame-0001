using System;
using UnityEngine;

namespace DSFramework {

    public class DSRay {
        public DSRay() { hit = new RaycastHit(); }

        #region 单条射线

        /// <summary>
        /// 射线碰撞检测
        /// </summary>
        private RaycastHit hit;

        /// <summary>
        /// 创建一条射线
        /// </summary>
        /// <param name="transform">发出射线的对象</param>
        /// <param name="origin">起点</param>
        /// <param name="direction">方向</param>
        /// <param name="maxDistance">最大距离</param>
        public void DSingleRay(Transform transform, Vector3 origin, Vector3 direction, float maxDistance) {
            DSInitSingleRay(transform, origin, direction, out hit, maxDistance);
        }

        /// <summary>
        /// 初始化一条射线
        /// </summary>
        /// <param name="transform">发出射线的对象</param>
        /// <param name="origin">起点</param>
        /// <param name="direction">方向</param>
        /// <param name="hitInfo">射线信息</param>
        /// <param name="maxDistance">最大距离</param>
        private void DSInitSingleRay(Transform transform, Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance) {
            //当检测到碰撞体时，则触发距离检测
            if (Physics.Raycast(origin, transform.TransformDirection(direction), out hitInfo, maxDistance)) {
                Debug.DrawRay(transform.position, transform.TransformDirection(direction) * hitInfo.distance, Color.yellow);
                //txt.text = $"当前距离：{hitInfo.distance}";
                Debug.Log(hitInfo.collider.name);
            } else {
                Debug.DrawRay(transform.position, transform.TransformDirection(direction) * 1000, Color.white);
            }
        }

        #region 常用方法

        /// <summary>
        /// 获取命中的碰撞体
        /// </summary>
        public Collider DSGetCollider => hit.collider;

        /// <summary>
        /// 获取命中的transform
        /// </summary>
        public Transform DSGetTrans => hit.transform;

        /// <summary>
        /// 获取命中的对象
        /// </summary>
        public GameObject DSGetGameObj => hit.transform.gameObject;

        /// <summary>
        /// 获取射线的距离
        /// </summary>
        public float DSDistance => hit.distance;

        /// <summary>
        /// 碰撞体名称
        /// </summary>
        public string DSName => DSGetCollider.name;

        /// <summary>
        /// 获取点击坐标
        /// </summary>
        public Vector3 DSPoint => hit.point;

        #endregion

        #endregion

        #region 根据 Layer

        /// <summary>
        /// 初始化层级射线
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="direction"></param>
        /// <param name="hitInfo"></param>
        /// <param name="maxDistance"></param>
        /// <param name="layerMask"></param>
        private void DSInitLayerRay(Transform transform, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask) {
            //当检测到碰撞体时，则触发距离检测
            if (Physics.Raycast(transform.position, transform.TransformDirection(direction), out hitInfo, maxDistance, layerMask)) {
                Debug.DrawRay(transform.position, transform.TransformDirection(direction) * hitInfo.distance, Color.yellow);
            } else {
                Debug.DrawRay(transform.position, transform.TransformDirection(direction) * 1000, Color.white);
            }

            #endregion

            #region 多条射线

            #endregion
        }

        /// <summary>
        /// 创建层级射线
        /// </summary>
        /// <param name="transform">发射的对象</param>
        /// <param name="direction">方向</param>
        /// <param name="maxDistance">最大距离</param>
        /// <param name="layerMask">碰撞的层级</param>
        public void DSLayerRay(Transform transform, Vector3 direction, float maxDistance, int layerMask) {
            DSInitLayerRay(transform, direction, out hit, maxDistance, layerMask);
        }
    }

}