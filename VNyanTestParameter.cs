using System;
using System.Collections.Generic;
using System.Text;
using VNyanInterface;
using UnityEngine;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace VNyanEmulator
{

    public class VNyanTestParameter : IParameterInterface
    {

        private static readonly VNyanTestParameter _instance = new();
        public static VNyanTestParameter Instance { get => _instance; }

        private static readonly Dictionary<string, string> VNyanStringParameters = [];
        private static readonly Dictionary<string, float> VNyanFloatParameters = [];
        private static readonly Dictionary<string, Dictionary<string, string>> VNyanDictionaries = [];

        public static Dictionary<string, string> StringParameters { get => new(VNyanStringParameters); }
        public static event Action<string, string, string> StringParameterChanged;

        public static Dictionary<string, float> FloatParameters { get => new(VNyanFloatParameters); }
        public static event Action<string, float, float> FloatParameterChanged;

        public static Dictionary<string, Dictionary<string, string>> Dictionaries { get => new(VNyanDictionaries); }
        public static event Action<string, string, string, string> DictionaryValueChanged;
        public static event Action<string> DictionaryCleared;

        static VNyanTestParameter() {}
        private VNyanTestParameter() {}

        public void setVNyanParameterString(string parameterName, string value)
        {
            Logger.LogInfo($" [Parameter] Setting the string parameter named {parameterName} with a value of {value}");
            VNyanStringParameters.TryGetValue(parameterName, out string oldValue);
            VNyanStringParameters[parameterName] = value;
            StringParameterChanged.Invoke(parameterName, value, oldValue);
        }

        public string getVNyanParameterString(string parameterName)
        {
            if (VNyanStringParameters.TryGetValue(parameterName, out string loadedParameter)) return loadedParameter;
            return "";
        }

        public void setVNyanParameterFloat(string parameterName, float value)
        {
            Logger.LogInfo($" [Parameter] Setting the float parameter named {parameterName} with a value of {value}");
            VNyanFloatParameters.TryGetValue(parameterName, out float oldValue);
            VNyanFloatParameters[parameterName] = value;
            FloatParameterChanged.Invoke(parameterName, value, oldValue);
        }

        public float getVNyanParameterFloat(string parameterName)
        {
            if (VNyanFloatParameters.TryGetValue(parameterName, out float loadedParameter)) return loadedParameter;
            return 0.0f;
        }

        public string fillStringWithParameters(string originalString)
        {
            //need to learn the expected output for this
            Logger.LogInfo($"Filling a string with parameters (unimplemented)");
            return "";
        }

        public string getVNyanDictionaryValue(string dictionaryName, string keyName)
        {
            if (!VNyanDictionaries.ContainsKey(dictionaryName)) return "";
            if (!VNyanDictionaries[dictionaryName].ContainsKey(keyName)) return "";
            return VNyanDictionaries[dictionaryName][keyName];
        }

        public void setVNyanDictionaryValue(string dictionaryName, string keyName, string value)
        {
            Logger.LogInfo($" [Parameter] Setting the key {keyName} in the dictionary named {dictionaryName} with a value of {value}");
            if (!VNyanDictionaries.ContainsKey(dictionaryName)) VNyanDictionaries.Add(dictionaryName, []);
            VNyanDictionaries[dictionaryName].TryGetValue(keyName, out string oldValue);
            VNyanDictionaries[dictionaryName][keyName] = value;
            DictionaryValueChanged.Invoke(dictionaryName, keyName, value, oldValue);
        }

        public void clearVNyanDictionary(string dictionaryName)
        {
            Logger.LogInfo($" [Parameter] Clearing the dictionary named {dictionaryName}");
            if (!VNyanDictionaries.ContainsKey(dictionaryName)) return;
            VNyanDictionaries.Remove(dictionaryName);
            DictionaryCleared.Invoke(dictionaryName);
        }

    }
}
