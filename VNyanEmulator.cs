using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
using UnityEditor.VersionControl;
using System.Collections.Generic;



namespace VNyanEmulator
{
    [InitializeOnLoad]
    public static class VNyanEmulator
    {

        public static event Action OnUpdate = new(() => { });

        public static void InvokeUpdate() => OnUpdate.Invoke();

        static VNyanEmulator()
        {
            VNyanInterface.VNyanInterface.VNyanTrigger ??= VNyanTestTrigger.Instance;
            VNyanInterface.VNyanInterface.VNyanParameter ??= VNyanTestParameter.Instance;
            VNyanInterface.VNyanInterface.VNyanSettings ??= VNyanTestSettings.Instance;
            VNyanInterface.VNyanInterface.VNyanUI ??= VNyanTestUI.Instance;
            VNyanInterface.VNyanInterface.VNyanAvatar ??= VNyanTestAvatar.Instance;
            VNyanInterface.VNyanInterface.VNyanRender ??= VNyanTestRender.Instance;
            VNyanInterface.VNyanInterface.VNyanPendulum ??= VNyanTestPendulum.Instance;

            OnUpdate += VNyanTestTrigger.CycleQueue;
            EditorApplication.playModeStateChanged += HandlePlayModeState;
        }

        public static void LoadCanvasBundle()
        {
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(VNyanEmulator), "canvas");
            
            byte[] bundleData = new byte[stream.Length];
            stream.Read(bundleData, 0, bundleData.Length);
            AssetBundle canvasBundle = AssetBundle.LoadFromMemory(bundleData);
            GameObject canvasProto = canvasBundle.LoadAsset<GameObject>(canvasBundle.GetAllAssetNames()[0]);
            GameObject.Instantiate(canvasProto);
        }

        private static void HandlePlayModeState(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode) LoadCanvasBundle();
        }
    }
}
