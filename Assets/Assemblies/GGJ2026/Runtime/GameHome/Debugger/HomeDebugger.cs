using System;
using DebugUI;
using GGJ2026.GamePlay;
using UnityEngine.UIElements;

namespace GGJ2026.Home
{
    public class HomeDebugger : DebugUIBuilderBase
    {
        protected override void Configure(IDebugUIBuilder builder)
        {
        }

        private void OnEnable()
        {
            GetComponent<UIDocument>().enabled = true;
        }

        private void OnDisable()
        {
            GetComponent<UIDocument>().enabled = false;
        }
    }
}