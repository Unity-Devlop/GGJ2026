using System;
using UnityEngine;
using UnityEngine.UI;
using UnityToolkit;

namespace Jump.GamePlay
{
    public class GamePlayPanel : UIPanel
    {
        [SerializeField] private Button backHomeButton;

        private void Awake()
        {
            backHomeButton.onClick.AddListener(OnBackHomeButtonClicked);
        }

        private void OnBackHomeButtonClicked()
        {
            Global.gameFlow.stateMachine.Change<HomeState>();
        }
    }
}