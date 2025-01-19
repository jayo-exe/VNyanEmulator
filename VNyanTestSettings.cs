using System;
using System.Collections.Generic;
using System.Text;
using VNyanInterface;
using UnityEngine;
using System.Runtime.InteropServices.ComTypes;

namespace VNyanEmulator
{

    public class VNyanTestSettings : ISettingsInterface
    {
        private static readonly VNyanTestSettings _instance = new();
        public static VNyanTestSettings Instance { get => _instance; }

        private static readonly Dictionary<string, Dictionary<string, string>> pluginSettingsData = [];
        static VNyanTestSettings() { }
        private VNyanTestSettings() { }

        public void saveSettings(string fileName, Dictionary<string, string> newPluginSettingsData)
        {
            Logger.LogInfo($" [Settings] Saving settings for {fileName} (unimplemented)");
            pluginSettingsData[fileName] = newPluginSettingsData;
        }

        public Dictionary<string, string> loadSettings(string fileName)
        {
            Logger.LogInfo($" [Settings] Loading settings for {fileName} (unimplemented)");
            if (pluginSettingsData.TryGetValue(fileName, out Dictionary<string, string> loadedPluginSettingsData))
            {
                return loadedPluginSettingsData;
            }
            return [];
        }

        public string getProfilePath()
        {
            Logger.LogInfo($" [Settings] Getting profile path (unimplemented)");
            return "";
        }

    }
}
