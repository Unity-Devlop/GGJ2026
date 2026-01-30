using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityToolkit;

namespace Jump.Home
{
    public class HomePanel : UIPanel
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Button startButton;
        [SerializeField] private HomeDebugger debugger;

        private void Awake()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
#if DEVELOPMENT
            debugger.enabled = true;
#else
            debugger.enabled = false;
#endif
        }

        private void OnStartButtonClicked()
        {
            Global.gameFlow.stateMachine.Change<GameState>();
        }
    }
}