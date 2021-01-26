using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 字体颜色类
/// </summary>

namespace DSFramework {

    public static class DSColor {
        /// <summary>
        /// 蓝色字体
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string Blue(this object obj) { return $"<color=#0000ff>{obj}</color>"; }

        /// <summary>
        /// 黄色字体
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>	
        public static string Yellow(this object obj) { return $"<color=#FFFF00>{obj}</color>"; }

        /// <summary>
        /// 猩红色字体
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string Red(this object obj) { return $"<color=#DC143C>{obj}</color>"; }

        /// <summary>
        /// 适中的碧绿色	
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string Green(this object obj) { return $"<color=#00FA9A>{obj}</color>"; }

        /// <summary>
        /// 纯白色
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string Write(this object obj) { return $"<color=#FFFFFF>{obj}</color>"; }

        /// <summary>
        /// 烟白色
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string WriteSmoke(this object obj) { return $"<color=#F5F5F5>{obj}</color>"; }

        /// <summary>
        /// 柠檬薄纱	
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string LemonChiffon(this object obj) { return $"<color=#FFFACD>{obj}</color>"; }

        /// <summary>
        /// 橙红色	
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string OrangeRed(this object obj) { return $"<color=#FF4500>{obj}</color>"; }

        /// <summary>
        /// 番茄
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Tomato(this object obj) { return $"<color=#FF6347>{obj}</color>"; }

        /// <summary>
        /// 金色
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Gold(this object obj) { return $"<color=#FFD700>{obj}</color>"; }
    }

}