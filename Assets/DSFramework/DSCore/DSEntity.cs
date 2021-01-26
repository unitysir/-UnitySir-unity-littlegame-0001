using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DSFramework
{
    public class DSEntity : MonoBehaviour
    {
        #region InitCmpt

        private static readonly LinkedList<DSComponent> m_BaseComponent = new LinkedList<DSComponent>();

        public static ResourceComponent Resource { get; private set; }
        public static SceneComponent Scene { get; private set; }
        public static DBComponent DB { get; private set; }
        public static UIComponent UI { get; private set; }
        public static AudioComponent Audio { get; private set; }
        public static PoolComponent Pool { get; private set; }
        public static MonoComponent Mono { get; private set; }
        public static MsgMechainComponent MsgMechain { get; private set; }
        public static DebugsComponent Debugs { get; private set; }

        #endregion

        private void Awake()
        {
            DontDestroyOnLoad(this);
            InitCmpts();
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        private void InitCmpts()
        {
            Transform cmpt = transform.GetChild(0);
            Resource = cmpt.DSGetChildCmpt4Trans<ResourceComponent>("Resource");
            DB = cmpt.DSGetChildCmpt4Trans<DBComponent>("DB");
            UI = cmpt.DSGetChildCmpt4Trans<UIComponent>("UI");
            Audio = cmpt.DSGetChildCmpt4Trans<AudioComponent>("Audio");
            Pool = cmpt.DSGetChildCmpt4Trans<PoolComponent>("Pool");
            Mono = cmpt.DSGetChildCmpt4Trans<MonoComponent>("Mono");
            MsgMechain = cmpt.DSGetChildCmpt4Trans<MsgMechainComponent>("MsgMechain");
            Scene = cmpt.DSGetChildCmpt4Trans<SceneComponent>("Scene");
            Debugs = cmpt.DSGetChildCmpt4Trans<DebugsComponent>("Debugs");
        }

        private void OnAwake()
        {
            Mono.InitCmpts();
            Resource.InitCmpts();
            Scene.InitCmpts();
            Debugs.InitCmpts();
            MsgMechain.InitCmpts();
            Pool.InitCmpts();
            DB.InitCmpts();
            Audio.InitCmpts();
            UI.InitCmpts();
        }

        private void OnStart()
        {
        }

        private void OnDestroy()
        {
            Mono.ShutDown();
            MsgMechain.ShutDown();
            Resource.ShutDown();
            Debugs.ShutDown();
            DB.ShutDown();
            UI.ShutDown();
            Audio.ShutDown();
            Scene.ShutDown();
            Pool.ShutDown();
            Debugs.ShutDown();
        }
    }
}