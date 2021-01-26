using System;
using System.IO;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace DSFramework
{
#if UNITY_EDITOR
    public class DSUtilityTool
    {
        private static string mPath = Application.dataPath;

        public static string GeneratePackageName(string name)
        {
            return name + "_" + DateTime.Now.ToString("yyyy-MM-dd__HH_mm_ss");
        }


        [MenuItem("DSFramework/01.导出 Unity Package %e", false, 1)]
        static void MenuExportClicked()
        {
            DSEditorUtil.ExportPackage("Assets/DSFramework",
                GeneratePackageName("DSFramework") + ".unitypackage");
            DSEditorUtil.ExportPackage("Assets/Scripts",
                GeneratePackageName("DSFramework_HotUpdate") + ".unitypackage");
            DSEditorUtil.OpenFile(Path.Combine(mPath, "../"));
        }


        [MenuItem("DSFramework/02.批量导入资源包", false, 2)]
        static void BatchImporter()
        {
            try
            {
                mPath = EditorUtility.OpenFolderPanel("选择批量导入文件夹", mPath, "");
                string[] files = Directory.GetFiles(mPath);
                foreach (string file in files)
                    if (file.EndsWith(".unitypackage"))
                        AssetDatabase.ImportPackage(file, false);
            }
            catch (Exception e)
            {
                Debug.LogError($"资源导入失败:{e.Message}");
            }
        }

        [MenuItem("DSFramework/03.打开文件所在位置", false, 3)]
        private static void MenuClicked()
        {
            DSEditorUtil.CallMenuItem();
        }

        [MenuItem("DSFramework/04.重新生成文件", false, 4)]
        public static void ReloadMetaFile()
        {
            DSEditorUtil.ReloadMetaFile();
        }


        #region 代码生成器

        [MenuItem("DSFramework/DSTool/1.生成代码但粘贴板", false, 0)]
        static void CodeBuilder()
        {
        }

        [InitializeOnLoadMethod]
        static void InitializeOnLoadMethod()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            if (Event.current != null && selectionRect.Contains(Event.current.mousePosition) &&
                Event.current.button == 1 && Event.current.type <= EventType.MouseUp) //
            {
                GameObject selectedGameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
                if (Event.current.control && selectedGameObject)
                {
                    Vector2 mousePosition = Event.current.mousePosition;

                    EditorUtility.DisplayPopupMenu(new Rect(mousePosition, Vector2.zero), "DSFramework/DSTool/", null);
                    Event.current.Use();

                    ObjToCode(selectedGameObject);
                }
            }
        }

        /// <summary>
        /// 通过对象生成代码
        /// </summary>
        /// <param name="selectedGameObject"></param>
        static void ObjToCode(GameObject selectedGameObject)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                //代码生成到粘贴板
                Transform[] children = selectedGameObject.GetComponentsInChildren<Transform>(true);

                foreach (Transform childTrans in children)
                {
                    string childName = childTrans.name; //获取到对象名称
                    string[] childNameArr = childName.Split('_');
                    try
                    {
                        if (childNameArr[0].Equals("DS"))
                        {
                            sb.Append($"[SerializeField] private {childNameArr[1]} {childName};\n");
                        }
                    }
                    catch
                    {
                        Debug.Log($"{childName}:不是目标组件");
                    }
                }

                GUIUtility.systemCopyBuffer = sb.ToString(); //将内容复制到剪贴板
                Debug.Log("代码生成成功！！！");
            }
            catch (Exception e)
            {
                Debug.LogError($"代码生成失败:{e.Message}");
            }
        }

        #endregion

        #region 创建 DSGameEntity

        [MenuItem("DSFramework/DSTool/2.生成DSEntity", true, 1)]
        static bool CMenuItem()
        {
            return !GameObject.Find("DSEntity");
        }

        [MenuItem("DSFramework/DSTool/2.生成DSEntity")]
        static void CCreateCanvas()
        {
            var DSGameEntity = new GameObject("DSEntity");
            DSGameEntity.tag = DSGameEntity.name;
            Debug.Log("DSGame--Tag=" + DSGameEntity.tag);

            var component = new GameObject("Component");
            component.DSetParent(DSGameEntity);

            var DB = new GameObject("DB");
            DB.DSetParent(component);

            var UI = new GameObject("UI");
            UI.layer = LayerMask.NameToLayer("UI");
            UI.DSetParent(component);

            var Resource = new GameObject("Resource");
            Resource.DSetParent(component);

            var Scene = new GameObject("Scene");
            Scene.DSetParent(component);

            var Audio = new GameObject("Audio");
            Audio.DSetParent(component);

            var Mono = new GameObject("Mono");
            Mono.DSetParent(component);

            var Pool = new GameObject("Pool");
            Pool.DSetParent(component);

            var MsgMechain = new GameObject("MsgMechain");
            MsgMechain.DSetParent(component);

            var Debugs = new GameObject("Debugs");
            Debugs.DSetParent(component);

            // ---------------------------------------------------------------------------

            var UIRoot = new GameObject("UIRoot");
            UIRoot.DSetParent(UI);
            UIRoot.GetOrAddCmpt<RectTransform>();
            var canvas = UIRoot.GetOrAddCmpt<Canvas>();
            var canvasScaler = UIRoot.GetOrAddCmpt<CanvasScaler>();
            var graphicRaycaster = UIRoot.GetOrAddCmpt<GraphicRaycaster>();
            UIRoot.tag = UIRoot.name;
            UIRoot.layer = LayerMask.NameToLayer("UI");
            Debug.Log("UIRoot--Tag=" + UIRoot.tag);

            var EventSystem = new GameObject("EventSystem");
            EventSystem.layer = LayerMask.NameToLayer("UI");
            EventSystem.DSetParent(UI);
            EventSystem.GetOrAddCmpt<RectTransform>();
            var eventSystem = EventSystem.GetOrAddCmpt<EventSystem>();
            var standaloneInputModel = EventSystem.GetOrAddCmpt<StandaloneInputModule>();

            var UICamera = new GameObject("UICamera");
            UICamera.layer = LayerMask.NameToLayer("UI");
            UICamera.DSetParent(UI);
            UICamera.GetOrAddCmpt<RectTransform>();
            var uiCamera = UICamera.GetOrAddCmpt<Camera>();

            var Background = new GameObject("Background");
            Background.layer = LayerMask.NameToLayer("UI");
            Background.DSetParent(UIRoot);
            RectTransform rect = Background.GetOrAddCmpt<RectTransform>();
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;

            var Normal = new GameObject("Normal");
            Normal.layer = LayerMask.NameToLayer("UI");
            Normal.DSetParent(UIRoot);
            rect = Normal.GetOrAddCmpt<RectTransform>();
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;

            var Tip = new GameObject("Tip");
            Tip.layer = LayerMask.NameToLayer("UI");
            Tip.DSetParent(UIRoot);
            rect = Tip.GetOrAddCmpt<RectTransform>();
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;

            var Update = new GameObject("Update");
            Update.DSetParent(Mono);
            Update.GetOrAddCmpt<RectTransform>();

            var LateUpdate = new GameObject("LateUpdate");
            LateUpdate.DSetParent(Mono);
            LateUpdate.GetOrAddCmpt<RectTransform>();

            var FixedUpdate = new GameObject("FixedUpdate");
            FixedUpdate.DSetParent(Mono);
            FixedUpdate.GetOrAddCmpt<RectTransform>();

            //-------------------------------------------------------------

            uiCamera.clearFlags = CameraClearFlags.Depth;
            uiCamera.cullingMask = LayerMask.GetMask("UI");
            uiCamera.orthographic = true;

            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = uiCamera;
            uiCamera.depth = 3;

            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1334, 750);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 1;
            canvasScaler.referencePixelsPerUnit = 300;

            // ----------------------------------------------------------------------------------

            DSGameEntity.GetOrAddCmpt<AudioListener>();
            DSGameEntity.GetOrAddCmpt<DSEntity>();
            DB.GetOrAddCmpt<DBComponent>();
            UI.GetOrAddCmpt<UIComponent>();
            Resource.GetOrAddCmpt<ResourceComponent>();
            Scene.GetOrAddCmpt<SceneComponent>();
            Audio.GetOrAddCmpt<AudioComponent>();
            Mono.GetOrAddCmpt<MonoComponent>();
            Mono.GetOrAddCmpt<DSMonoController>();
            Pool.GetOrAddCmpt<PoolComponent>();
            MsgMechain.GetOrAddCmpt<MsgMechainComponent>();
            Debugs.GetOrAddCmpt<DebugsComponent>();

            SavePrefab(DSGameEntity, "DSFramework/DSRes/Prefabs/UI", "DSEntity");
        }

        /// <summary>
        /// 将对象保存为 Prefab
        /// </summary>
        /// <param name="obj">需要保存的对象</param>
        /// <param name="fileDir">Prefab保存的目录</param>
        /// <param name="prefabName">预设体名称</param>
        static void SavePrefab(GameObject obj, string fileDir, string prefabName)
        {
            var currPath = Application.dataPath + "/" + fileDir + "/";
            if (!Directory.Exists(currPath))
            {
                Directory.CreateDirectory(currPath);
            }

            var saveFilePath = currPath + prefabName + ".prefab"; //保存文件的路径
            PrefabUtility.SaveAsPrefabAssetAndConnect(obj, saveFilePath, InteractionMode.AutomatedAction);
        }

        #endregion
    }
#endif
}