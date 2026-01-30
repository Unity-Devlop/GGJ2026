using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    public class GamePlayPanel : UIPanel
    {
        [SerializeField] private UICardContainer cardContainer;
        [SerializeField] private RectTransform useCardArea;


        private PlayerData _playerData;

        public void Bind(PlayerData playerData)
        {
            _playerData = playerData;
            cardContainer.Bind(playerData);

            Global.Event.Listen<OnUICardVisualEndDrag>(OnUICardVisualEndDrag);
        }

        public void UnBind()
        {
            Global.Event.UnListen<OnUICardVisualEndDrag>(OnUICardVisualEndDrag);
            cardContainer.UnBind();
            _playerData = null;
        }

        private void OnUICardVisualEndDrag(in OnUICardVisualEndDrag args)
        {
            Debug.Log("OnUICardVisualEndDrag");
            // 看看是否在使用区域
            if (RectTransformUtility.RectangleContainsScreenPoint(useCardArea, Pointer.current.position.value))
            {
                Debug.Log($"Use Card: {args.data}");
            }
        }
    }
}