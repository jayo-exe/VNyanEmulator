using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VNyanInterface;

namespace VNyanEmulator
{
    public class VNyanPluginLoader : MonoBehaviour
    {
        private static readonly Dictionary<string, IVNyanPluginManifest> loadedPlugins = [];
        public static Dictionary<string, IVNyanPluginManifest> LoadedPlugins { get { return new(loadedPlugins); } }

        public void Start()
        {
            Debug.Log($"[Plugin Loader] Looking for manifests");
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types = [];
                try
                {
                    types = assembly.GetTypes();
                } catch(System.Reflection.ReflectionTypeLoadException e)
                {
                    foreach(Exception ee in e.LoaderExceptions) {
                        //Debug.Log(ee.Message);
                    }
                    
                }
                foreach (var type in types)
                {
                    if (!type.GetInterfaces().Contains(typeof(IVNyanPluginManifest))) continue;
                    
                    try
                    {
                        IVNyanPluginManifest manifestInstance = (IVNyanPluginManifest)Activator.CreateInstance(type);
                        Logger.LogInfo($"Found a plugin manifest: {type.FullName} \n name: {manifestInstance.PluginName} | version: {manifestInstance.Version} | title: {manifestInstance.Title} | author: {manifestInstance.Author}");
                        loadedPlugins.Add(manifestInstance.PluginName, manifestInstance);
                        manifestInstance.InitializePlugin();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogInfo($"Error initializing plugin from {type.FullName}: {ex.Message}");
                    }
                }
            }
        }
    }
}
