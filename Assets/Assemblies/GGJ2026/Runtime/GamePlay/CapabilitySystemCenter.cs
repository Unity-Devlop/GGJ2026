using UnityToolkit;
using UnityEngine;

namespace Capabilities
{
    public sealed class CapabilitySystemCenter : MonoSingleton<CapabilitySystemCenter>
    {
        protected override bool DontDestroyOnLoad() => true;
        [Sirenix.OdinInspector.ShowInInspector]
        public CapabilitySystem capabilitySystem { get; private set; }

        protected override void OnInit()
        {
            capabilitySystem = new CapabilitySystem();
            capabilitySystem.OnInit();
        }

        protected override void OnDispose()
        {
            capabilitySystem.OnDispose();
        }

        private void Update()
        {
            capabilitySystem.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            capabilitySystem.FixedUpdate(Time.fixedDeltaTime);
        }

        public void Register(ICapability capability)
        {
            capabilitySystem.Register(capability);
        }

        public void Unregister(ICapability capability)
        {
            capabilitySystem.Unregister(capability);
        }
    }
}