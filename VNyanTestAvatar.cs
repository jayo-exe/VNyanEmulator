using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Linq;
using UnityEngine;
using VNyanInterface;

namespace VNyanEmulator
{
    public class VNyanTestAvatar : IAvatarInterface
    {
        private static readonly VNyanTestAvatar _instance = new();
        public static VNyanTestAvatar Instance { get => _instance; }

        private static GameObject avatar;
        private static readonly Dictionary<string, float> blendshapeOverrides = [];
        private static readonly Dictionary<string, float> meshBlendshapeOverrides = [];
        private static readonly Dictionary<string, float> blendshapes = [];
        private static readonly Dictionary<string, float> blendshapesLastFrame = [];
        private static readonly List<IPoseLayer> poseLayers = [];
        private static readonly List<IBlendshapeProcessingListener> blendshapeProcessingListeners = [];

        static VNyanTestAvatar() { }
        private VNyanTestAvatar() { }

        public static void SetAvatar(GameObject newAvatar)
        {
            Logger.LogInfo($" [Avatar] Setting Avatar to {(newAvatar == null ? "None!" : newAvatar.name)}");
            avatar = newAvatar;
        }

        public void setBlendshapeOverride(string name, float value)
        {
            Logger.LogInfo($" [Avatar] Setting the blendshape override for {name} with a value of {value}");
            blendshapeOverrides.Add(name, value);
        }

        public void clearBlendshapeOverride(string name)
        {
            Logger.LogInfo($" [Avatar] Clearing the blendshape override for {name}");
            blendshapeOverrides.Remove(name);
        }

        public void setMeshBlendshapeOverride(string name, float value)
        {
            Logger.LogInfo($" [Avatar] Setting the mesh blendshape override for {name} with a value of {value}");
            meshBlendshapeOverrides.Add(name, value);
        }

        public void clearMeshBlendshapeOverride(string name)
        {
            Logger.LogInfo($" [Avatar] Clearing the mesh blendshape override for {name}");
            meshBlendshapeOverrides.Remove(name);
        }

        public void registerBlendshapeProcessingListener(IBlendshapeProcessingListener listener)
        {
            Logger.LogInfo($" [Avatar] Registering a blendshape processing listener: {listener.GetType().FullName}");
            blendshapeProcessingListeners.Add(listener);
        }

        public void registerPoseLayer(IPoseLayer layer)
        {
            Logger.LogInfo($" [Avatar] Registering a pose layer: {layer.GetType().FullName}");
            poseLayers.Add(layer);
        }

        public object getAvatarObject()
        {
            return avatar;
        }

        public float getBlendshapeInstant(string name)
        {
            Logger.LogInfo($" [Avatar] Getting instant blendshape value for {name} (unimplemented)");
            blendshapes.TryGetValue(name, out float foundValue);
            return foundValue;
        }

        public float getBlendshapeLastFrame(string name)
        {
            Logger.LogInfo($" [Avatar] Getting last-frame blendshape value for {name} (unimplemented)");
            blendshapesLastFrame.TryGetValue(name, out float foundValue);
            return foundValue;
        }

        public Dictionary<string, float> getBlendshapesInstant()
        {
            Logger.LogInfo($" [Avatar] Getting instant blendshapes list (unimplemented)");
            return new(blendshapes);
        }

    }
}
