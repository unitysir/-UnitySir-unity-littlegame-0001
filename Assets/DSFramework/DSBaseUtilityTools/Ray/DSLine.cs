using UnityEngine;

namespace DSFramework {

    /// <summary>
    /// 画直线类
    /// </summary>
    public class DSLine {
        /// <summary>
        /// 创建默认直线
        /// </summary>
        /// <param name="lineRenderer">直线渲染组件</param>
        /// <param name="transform">直线起始位置</param>
        public void DSCreateDefaultLine(LineRenderer lineRenderer, Transform transform,float distance) {
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = 0.02f;
            lineRenderer.endWidth = 0.02f;

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.forward * distance);
        }

        /// <summary>
        /// 创建直线
        /// </summary>
        /// <param name="lineRenderer">直线渲染组件</param>
        /// <param name="vexCount">生成直线的定点个数</param>
        /// <param name="width">直线宽度</param>
        /// <param name="transform">直线起始位置</param>
        /// <param name="distance">距离</param>
        public void DSCreateLine(
            LineRenderer lineRenderer,
            int vexCount,
            float width,
            Transform transform,
            float distance) {
            lineRenderer.positionCount = vexCount;
            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.forward * distance);
        }
    }

}