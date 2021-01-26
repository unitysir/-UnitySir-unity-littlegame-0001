using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DSFramework;
using UnityEngine;

namespace DSFramework
{
    public class DSLog
    {
        public static string LogErrorRootPath => Application.persistentDataPath + "/LogError/";
        static string logErrorPath;
        static StringBuilder allLogStr = new StringBuilder();
        static StreamWriter logWrite;


        /// <summary>
        /// 封装系统Debug.Log
        /// </summary>
        /// <param name="log">输出的日志内容</param>
        /// <param name="isAppend">是否对日志进行保留拼接</param>
        public static void I(object log, bool isAppend = true)
        {
            if (DSEntity.Debugs.OpenDebugLog)
                Debug.Log(log);

            if (isAppend)
                AppendLog(log);
        }

        public static void E(object log, bool containStackTrace = true)
        {
            string messageStr = log.ToString();
            if (DSEntity.Debugs.OpenDebugLog)
                Debug.LogError(log);
            LogToFile(messageStr, containStackTrace ? new System.Diagnostics.StackFrame().ToString() : null,
                LogType.Error);
        }

        private static void AppendLog(object log)
        {
            try
            {
                if (allLogStr.Length > 1048576)
                    allLogStr.Clear();
                allLogStr.AppendLine();
                allLogStr.Append($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}   {log}");
                allLogStr.AppendLine();
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        private static void LogToFile(string condition, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Exception:
                case LogType.Error:
                    RefreshOrCreateFile();
                    if (logWrite == null || !string.IsNullOrEmpty(stackTrace))
                    {
                        logWrite.Close();
                        File.Delete(logErrorPath);
                        return;
                    }

                    logWrite.WriteLine();
                    logWrite.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 错误类型:" + type);
                    logWrite.WriteLine(condition);
                    if (!string.IsNullOrEmpty(stackTrace))
                        logWrite.WriteLine(stackTrace);
                    //logWrite.WriteLine(type);
                    logWrite.Flush();
                    break;
                case LogType.Assert:
                case LogType.Log:
                case LogType.Warning:
                    break;
            }
        }

        private static void RefreshOrCreateFile()
        {
            if (string.IsNullOrEmpty(logErrorPath))
            {
                logErrorPath = LogErrorRootPath + ScriptTool.DateTimeToTimestampInMilliseconds(DateTime.Now) + ".log";
                ScriptTool.CreateTextFile(logErrorPath, "");
                logWrite = new StreamWriter(logErrorPath) {AutoFlush = false};
            }
            else
            {
                if (!File.Exists(logErrorPath))
                    return;
                logWrite?.Flush();
            }
        }
    }
}