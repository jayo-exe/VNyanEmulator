using System;
using System.Collections.Generic;
using VNyanInterface;
using UnityEngine;
using UnityEditor;

namespace VNyanEmulator
{

    public class VNyanTestUI : IUIInterface
    {
        private static readonly VNyanTestUI _instance = new();
        public static VNyanTestUI Instance { get => _instance; }

        private static Dictionary<string, IButtonClickedHandler> registeredPlugins = new();
        public static Dictionary<string, IButtonClickedHandler> RegisteredPlugins { get => new(registeredPlugins); }

        private static List<GameObject> registeredPrefabs = new();
        public static List<GameObject> RegisteredPrefabs { get => new(registeredPrefabs); }

        static VNyanTestUI() { }
        private VNyanTestUI() { }

        public static event Action<string> PluginRegistered;

        public static event Action<GameObject> PrefabInstantiated;

        public object instantiateUIPrefab(object gameObject)
        {
            if (!(gameObject is GameObject)) return null;
            GameObject window = GameObject.Instantiate((GameObject)gameObject);
            registeredPrefabs.Add(window);
            PrefabInstantiated?.Invoke(window);
            return window;
        }

        public void registerPluginButton(string buttonText, IButtonClickedHandler clickCallback)
        {
            registeredPlugins[buttonText] = clickCallback;
            PluginRegistered?.Invoke(buttonText);
        }

        public string openLoadFileDialog(string header, string[] extensions)
        {
            string path = EditorUtility.OpenFilePanel(header, "", String.Join(",", extensions));
            if (path.Length != 0) return path;
            return "";
        }

        public string openSaveFileDialog(string header, string[] extensions)
        {
            string path = EditorUtility.SaveFilePanel(header, "", "", String.Join(",", extensions));
            if (path.Length != 0) return path;
            return "";
        }

    }
}
