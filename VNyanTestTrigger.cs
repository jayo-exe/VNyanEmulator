using System;
using System.Collections.Generic;
using System.Text;
using VNyanInterface;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace VNyanEmulator
{

    public class VNyanTestTriggerData
    {
        public string triggerName = "";

        public int value1 = 0;
        public int value2 = 0;
        public int value3 = 0;

        public string text1 = "";
        public string text2 = "";
        public string text3 = "";
    }

    public class VNyanTestTrigger : ITriggerInterface
    {
        private static readonly VNyanTestTrigger _instance = new();
        public static VNyanTestTrigger Instance { get => _instance; }

        private static event Action<string, int, int, int, string, string, string> TriggerFired;
        private static readonly Queue<VNyanTestTriggerData> triggerQueue = [];

        static VNyanTestTrigger() { 
            TriggerFired += (string n, int v1, int v2, int v3, string t1, string t2, string t3) => {
                Logger.LogInfo($"[Trigger] Dequeueing trigger {n} \n {v1} | {v2} | {v3} | {t1} | {t2} | {t3}");
            }; 
        }
        private VNyanTestTrigger() { }

        public void registerTriggerListener(ITriggerHandler triggerHandler)
        {
            Logger.LogInfo($"[Trigger] Registering a trigger listener: {triggerHandler.GetType().FullName}");
            TriggerFired += triggerHandler.triggerCalled;
        }

        public void callTrigger(string triggerName, int value1, int value2, int value3, string text1, string text2, string text3)
        {
            Logger.LogInfo($"[Trigger] Enqueueing trigger {triggerName} \n {value1} | {value2} | {value3} | {text1} | {text2} | {text3}");

            lock (triggerQueue)
            {
                VNyanTestTriggerData trigger = new()
                {
                    triggerName = triggerName,
                    value1 = value1,
                    value2 = value2,
                    value3 = value3,
                    text1 = text1,
                    text2 = text2,
                    text3 = text3
                };
                triggerQueue.Enqueue(trigger);
            }
        }

        public static void CycleQueue()
        { 
            lock (triggerQueue)
            {
                while (triggerQueue.Count > 0)
                {
                    VNyanTestTriggerData trigger = triggerQueue.Dequeue();
                    TriggerFired.Invoke(trigger.triggerName, trigger.value1, trigger.value2, trigger.value3, trigger.text1, trigger.text2, trigger.text3);
                }
            }
        }
    }
}
