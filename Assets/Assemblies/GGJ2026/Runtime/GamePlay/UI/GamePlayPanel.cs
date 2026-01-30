using System;
using UnityEngine;
using UnityEngine.UI;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    public class GamePlayPanel : UIPanel
    {
        [SerializeField] private UICardContainer cardContainer;

        private PlayerData _playerData;

        public void Bind(PlayerData playerData)
        {
            _playerData = playerData;
            cardContainer.Bind(playerData);
        }
    }
}