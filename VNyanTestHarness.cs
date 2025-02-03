using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VNyanEmulator
{
    public class VNyanTestHarness : MonoBehaviour
    {
        private static VNyanTestHarness _instance;
        private static readonly Queue<Action> _actionQueue = [];

        private static readonly Dictionary<string, Dictionary<string, string>> pluginSettingsData = [];

        public GameObject buttonHolderObject;
        public GameObject stringParameterBox;
        public GameObject floatParameterBox;

        private DefaultControls.Resources uiResources;

        public void Awake()
        {

            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }

            gameObject.AddComponent<VNyanPluginLoader>();
            
            if (GameObject.FindObjectOfType<EventSystem>() == null)
            {
                var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }

            uiResources = new DefaultControls.Resources();
            foreach (Sprite sprite in Resources.FindObjectsOfTypeAll<Sprite>())
            {
                if (sprite.name == "UISprite")
                {
                    uiResources.standard = sprite;
                    break;
                }
            }

            VNyanTestParameter.StringParameterChanged += (string parameterName, string value, string oldValue) => { RefreshParameterDisplay(); };
            VNyanTestParameter.FloatParameterChanged += (string parameterName, float value, float oldValue) => { RefreshParameterDisplay(); };
            VNyanTestParameter.DictionaryValueChanged += (string dictionaryName, string keyName, string value, string oldValue) => { RefreshParameterDisplay(); };
            VNyanTestParameter.DictionaryCleared += (string dictionaryName) => { RefreshParameterDisplay(); };

            VNyanTestUI.PluginRegistered += (string buttonText) => { RefreshButtonDisplay(); };
            VNyanTestUI.PrefabInstantiated += (GameObject window) => { window.transform.SetParent(transform); };

            List<GameObject> extantPrefabs = VNyanTestUI.RegisteredPrefabs;
            foreach (GameObject prefab in extantPrefabs)
            {
                prefab.transform.SetParent(transform);
                prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            }
            RefreshButtonDisplay();
        }

        public void Start()
        {
            
        }

        public void Update()
        {
            VNyanEmulator.InvokeUpdate();

            if (_instance == null) return;

            lock (_actionQueue)
            {
                while (_actionQueue.Count > 0)
                {
                    _actionQueue.Dequeue().Invoke();
                }
            }
        }

        public static void OnMainThread(Action action)
        {
            if (_instance == null) return;

            lock (_actionQueue)
            {
                _actionQueue.Enqueue(action);
            }
        }

        public void RefreshButtonDisplay()
        {
            OnMainThread(() =>
            {
                var VNyanRegisteredPlugins = VNyanTestUI.RegisteredPlugins;

                int buttonWidth = 160;
                int buttonHeight = 24;
                int gap = 12;
                int gapOffset = 0;

                buttonHolderObject.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, gap, gap + gap + (gap + buttonHeight) * VNyanRegisteredPlugins.Keys.Count);
                buttonHolderObject.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, gap, gap + gap + buttonWidth);

                foreach (string key in VNyanRegisteredPlugins.Keys)
                {
                    GameObject pluginButton = DefaultControls.CreateButton(uiResources);
                    pluginButton.name = $"__VNyanPluginButton {key}";
                    pluginButton.transform.SetParent(buttonHolderObject.transform);
                    pluginButton.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, gap, buttonWidth);
                    pluginButton.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, gap + gapOffset, buttonHeight);
                    pluginButton.GetComponentInChildren<Text>().text = key;
                    pluginButton.GetComponent<Button>().onClick.AddListener(VNyanRegisteredPlugins[key].pluginButtonClicked);
                    gapOffset += gap + buttonHeight;
                }
            });
        }

        public void RefreshParameterDisplay()
        {
            OnMainThread(() => {
                int itemWidth = 160;
                int itemHeight = 14;
                int gap = 6;
                int gapOffset = 0;
                GameObject parameterItem;


                gapOffset = (gap + itemHeight * 2) * stringParameterBox.transform.childCount;
                var VNyanStringParameters = VNyanTestParameter.StringParameters;
                foreach (string key in VNyanStringParameters.Keys)
                {

                    parameterItem = GameObject.Find($"__VNyanStringParameterText-{key}");
                    if (!parameterItem)
                    {
                        parameterItem = DefaultControls.CreateText(uiResources);
                        parameterItem.name = $"__VNyanStringParameterText-{key}";
                        parameterItem.transform.SetParent(stringParameterBox.transform);
                        parameterItem.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, gap, itemWidth);
                        parameterItem.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, gap + gapOffset, itemHeight * 2);
                        parameterItem.GetComponentInChildren<Text>().fontSize = 12;
                        gapOffset += gap + itemHeight * 2;
                        stringParameterBox.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, gap + gapOffset);
                    }

                    parameterItem.GetComponentInChildren<Text>().text = $"{key}:\n{VNyanStringParameters[key]}";
                }


                gapOffset = (gap + itemHeight) * floatParameterBox.transform.childCount;
                var VNyanFloatParameters = VNyanTestParameter.FloatParameters;
                foreach (string key in VNyanFloatParameters.Keys)
                {
                    parameterItem = GameObject.Find($"__VNyanFloatParameterText-{key}");
                    if (!parameterItem)
                    {
                        parameterItem = DefaultControls.CreateText(uiResources);
                        parameterItem.name = $"__VNyanFloatParameterText-{key}";
                        parameterItem.transform.SetParent(floatParameterBox.transform);
                        parameterItem.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, gap, itemWidth);
                        parameterItem.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, gap + gapOffset, itemHeight);
                        parameterItem.GetComponentInChildren<Text>().fontSize = 12;
                        gapOffset += gap + itemHeight;
                        floatParameterBox.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, gap + gapOffset);
                    }

                    parameterItem.GetComponentInChildren<Text>().text = $"{key}: {VNyanFloatParameters[key]}";
                }
            });
        }
    }
}
