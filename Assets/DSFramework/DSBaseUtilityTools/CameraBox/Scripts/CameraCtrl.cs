using UnityEngine;

namespace DSFramework {

    public class CameraCtrl : MonoBehaviour {
        public static CameraCtrl Instance;

        /// <summary>
        /// 控制摄像机左右
        /// </summary>
        [SerializeField]
        private Transform m_RotationY;

        /// <summary>
        /// 控制摄像机上下
        /// </summary>
        [SerializeField]
        private Transform m_RotationX;

        /// <summary>
        /// 摄像机缩放父物体
        /// </summary>
        [SerializeField]
        private Transform m_PositionZ;

        /// <summary>
        /// 摄像机容器
        /// </summary>
        [SerializeField]
        private Transform m_Container;

        void Awake ( ) {
            Instance = this;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="far">摄像机远近</param>
        /// <param name="height">摄像机高度</param>
        public void Init ( Transform rolePos, float far, float height ) {
            AutoFollow(rolePos);
            m_PositionZ.localPosition = new Vector3(0f, 0f, far);
            m_RotationX.localRotation = Quaternion.Euler(height, 0f, 0f);
        }

        /// <summary>
        /// 相机实时跟随
        /// </summary>
        /// <param name="rolePos"></param>
        public void AutoFollow ( Transform rolePos ) {
            transform.position = rolePos.position;
            //transform.rotation = Quaternion.Euler(transform.rotation.x, rolePos.localEulerAngles.y ,transform.rotation.z);
        }

        /// <summary>
        /// 设置摄像机旋转
        /// </summary>
        /// <param name="type">1=左 0=右</param>
        public void SetCameraRotate ( int type ) {
            m_RotationY.transform.Rotate(0, 80 * Time.deltaTime * ( type == 0 ? -1 : 1 ), 0);
        }
        /// <summary>
        /// 设置摄像机旋转
        /// </summary>
        /// <param name="type">1=左 0=右</param>
        /// <param name="speed">旋转速度</param>
        public void SetCameraRotate ( int type,float speed ) {
            m_RotationY.transform.Rotate(0, speed * Time.deltaTime * ( type == 0 ? -1 : 1 ), 0);
        }

        /// <summary>
        /// 设置摄像机上下 
        /// </summary>
        /// <param name="type">1=上 0=下</param>
        public void SetCameraUpAndDown ( int type ) {
            m_RotationX.transform.Rotate(60 * Time.deltaTime * ( type == 0 ? -1 : 1 ), 0, 0);
            m_RotationX.transform.localEulerAngles = new Vector3(Mathf.Clamp(m_RotationX.transform.localEulerAngles.x, 25f, 60f), 0, 0);
        }
        /// <summary>
        /// 设置摄像机上下
        /// </summary>
        /// <param name="type">1=上 0=下</param>
        /// <param name="h">相机最高</param>
        /// <param name="l">相机最低</param>
        public void SetCameraUpAndDown ( int type,float h,float l ) {
            m_RotationX.transform.Rotate(60 * Time.deltaTime * ( type == 0 ? -1 : 1 ), 0, 0);
            m_RotationX.transform.localEulerAngles = new Vector3(Mathf.Clamp(m_RotationX.transform.localEulerAngles.x, l, h), 0, 0);
        }

        /// <summary>
        /// 设置摄像机 缩放
        /// </summary>
        /// <param name="type">0=拉近 1=拉远</param>
        /// <param name="far">最远</param>
        /// <param name="near">最近</param>
        public void SetCameraZoom ( int type,float far, float near ) {
            m_PositionZ.Translate(Vector3.forward * 10f * Time.deltaTime * ( ( type == 1 ? -1 : 1 ) ));
            m_PositionZ.localPosition = new Vector3(0, 0, Mathf.Clamp(m_PositionZ.localPosition.z, far, near));
        }
        /// <summary>
        /// 设置摄像机 缩放
        /// </summary>
        /// <param name="type">0=拉近 1=拉远</param>
        /// <param name="speed">移动速度</param>
        /// <param name="far">最远</param>
        /// <param name="near">最近</param>
        public void SetCameraZoom ( int type ,float speed, float far,float near) {
            m_PositionZ.Translate(Vector3.forward * speed * Time.deltaTime * ( ( type == 1 ? -1 : 1 ) ));
            m_PositionZ.localPosition = new Vector3(0, 0, Mathf.Clamp(m_PositionZ.localPosition.z, far, near));
        }
    }

}
