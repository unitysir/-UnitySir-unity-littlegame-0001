using System;
using System.IO;

namespace DSFramework
{
#if UNITY_EDITOR

    public class DScriptsInfoRecoder : UnityEditor.AssetModificationProcessor
    {
        private static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");
            if (path.EndsWith(".cs"))
            {
                string str = File.ReadAllText(path);
                str = str.Replace("UnitySir", Environment.UserName).Replace("#Date",
                    string.Concat(DateTime.Now.Year, "/", DateTime.Now.Month, "/", DateTime.Now.Day, " ",
                        DateTime.Now.Hour, ":", DateTime.Now.Minute, ":", DateTime.Now.Second));
                File.WriteAllText(path, str);
            }
        }
    }
#endif
}