using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using VNyanInterface;

namespace VNyanEmulator
{
    public class VNyanTestPendulum: IPendulumInterface
    {
        private static readonly VNyanTestPendulum _instance = new();
        public static VNyanTestPendulum Instance { get => _instance; }
        
        private static readonly List<IPendulumRoot> pendulumRoots = [];

        static VNyanTestPendulum() { }
        private VNyanTestPendulum() { }

        public IPendulumRoot createPendulumChain(int boneCount, float damping, float elasticity, float stiffness, float inert) 
        {
            Logger.LogInfo($" [Pendulum] Creating a pendulum chain \n boneCount: {boneCount} | damping: {damping} | elasticity {elasticity} | stiffness: {stiffness} | inert: {inert}");
            IPendulumRoot newRoot = new VNyanTestPendulumRoot(boneCount, damping, elasticity, stiffness, inert);
            if (!pendulumRoots.Contains(newRoot)) { pendulumRoots.Add(newRoot); }
            return newRoot;
        }

        public void deletePendulumChain(IPendulumRoot root) 
        {
            Logger.LogInfo($" [Pendulum] Deleting a pendulum chain");
            if (pendulumRoots.Contains(root)) { pendulumRoots.Remove(root); }
        }
    }

    public class VNyanTestPendulumRoot : IPendulumRoot
    {
        private readonly List<IPendulumChain> chains = [];

        private readonly int boneCount;
        private readonly float damping;
        private readonly float elasticity;
        private readonly float stiffness;
        private readonly float inert;
        private float positionValue = new();
        private float rotationValue = new();

        public VNyanTestPendulumRoot(int boneCount, float damping, float elasticity, float stiffness, float inert) {
            this.boneCount = boneCount;
            this.damping = damping;
            this.elasticity = elasticity;
            this.stiffness = stiffness;
            this.inert = inert;
        }
        public void setPositionValue(float value) { 
            positionValue = value;
            Logger.LogInfo($" [Pendulum Root] Setting position value for a pendulum root: \n boneCount: {boneCount} | damping: {damping} | elasticity {elasticity} | stiffness: {stiffness} | inert: {inert} | value: {positionValue}");
        }

        public void setRotationValue(float value) { 
            rotationValue = value;
            Logger.LogInfo($" [Pendulum Root] Setting position value for a pendulum root: \n boneCount: {boneCount} | damping: {damping} | elasticity {elasticity} | stiffness: {stiffness} | inert: {inert} | value: {rotationValue}");
        }

        public List<IPendulumChain> getChains() { return new(chains); }
    }
}
