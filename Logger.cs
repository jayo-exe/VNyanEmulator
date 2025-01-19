using UnityEngine;

namespace VNyanEmulator
{
    static class Logger
    {
        private static readonly string identifier = "VNyan Emulator";
        public static void LogInfo(object message) => Debug.Log($"[{identifier}] {message}");
        public static void LogWarning(object message) => Debug.LogWarning($"[{identifier}] {message}");
        public static void LogError(object message) => Debug.LogError($"[{identifier}] {message}");
    }
}