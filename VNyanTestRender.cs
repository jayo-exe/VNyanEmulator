using System;
using VNyanInterface;
using UnityEngine;
using System.Collections.Generic;

namespace VNyanEmulator
{
    public class VNyanTestRender: IRenderInterface
    {
        private static readonly List<IRenderEffect> renderEffects = [];
        private static readonly VNyanTestRender _instance = new();
        public static VNyanTestRender Instance { get => _instance; }

        static VNyanTestRender() { }
        private VNyanTestRender() { }

        public void registerRenderEffect(IRenderEffect effect) 
        {
            Logger.LogInfo($" [Render] Registering a render effect: {effect.GetType().FullName}");
            if (!renderEffects.Contains(effect)) renderEffects.Add(effect);
        }

    }
}
