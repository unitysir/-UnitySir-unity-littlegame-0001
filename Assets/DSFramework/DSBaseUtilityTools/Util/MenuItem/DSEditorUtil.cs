#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.IO;
using UnityEngine;

namespace DSFramework
{
    public partial class DSEditorUtil
    {
#if UNITY_EDITOR
        public static void OpenFile(string filepath)
        {
            Application.OpenURL("file:///" + filepath);
        }

        public static void CallMenuItem()
        {
            Application.OpenURL("file://" + Path.Combine(Application.dataPath, "../"));
        }

        public static void ExportPackage(string assetName, string fileName)
        {
            AssetDatabase.ExportPackage(assetName, fileName, ExportPackageOptions.Recurse);
        }

        /// <summary>
        /// 重新生成文件
        /// </summary>
        public static void ReloadMetaFile()
        {
            bool isSucc = DeleteAllFile(Path.Combine(Application.dataPath));
            Debug.Log(isSucc ? "生成成功!" : "生成失败!");
        }

        private static bool DeleteAllFile(string fullPath)
        {
            //获取指定路径下面的所有资源文件  然后进行删除
            if (Directory.Exists(fullPath))
            {
                Debug.Log("datapath = " + Application.dataPath);
                DirectoryInfo direction = new DirectoryInfo(fullPath);
                Debug.Log("direction = " + direction.FullName);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

                Debug.Log(files.Length);
                
                foreach (var t in files)
                {
                    if (t.Name.EndsWith(".meta"))
                    {
                        File.Delete(t.FullName);
                    }

                    Debug.Log(t.Name);
                }
                
                return true;
            }

            return false;
        }


#endif
    }
}