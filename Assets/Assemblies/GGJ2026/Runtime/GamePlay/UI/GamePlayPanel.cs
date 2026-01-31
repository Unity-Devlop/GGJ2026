using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    public class GamePlayPanel : UIPanel
    {
        [SerializeField] private UICardContainer cardContainer;
        [SerializeField] private RectTransform useCardArea;
        [SerializeField] private TurnStartUIEffect playerStartUIEffect;
        [SerializeField] private TurnStartUIEffect enemyStartUIEffect;

        private PlayerData _playerData;

        public void Bind(PlayerData playerData)
        {
            _playerData = playerData;
            cardContainer.Bind(playerData);

            Global.Event.Listen<OnUICardVisualEndDrag>(OnUICardVisualEndDrag);
            Global.Event.Listen<GamingMgr.GamingState>(OnGamingStateChanged);
        }

        private void OnGamingStateChanged(in GamingMgr.GamingState args)
        {
            switch (args)
            {
                case GamingMgr.GamingState.GameStart:
                    break;
                case GamingMgr.GamingState.PlayerRound:
                    playerStartUIEffect.PlayEffect().Forget();
                    break;
                case GamingMgr.GamingState.EnemyRound:
                    break;
                case GamingMgr.GamingState.GameOver:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(args), args, null);
            }
        }

        public void UnBind()
        {
            Global.Event.UnListen<OnUICardVisualEndDrag>(OnUICardVisualEndDrag);
            Global.Event.UnListen<GamingMgr.GamingState>(OnGamingStateChanged);
            cardContainer.UnBind();
            _playerData = null;
        }

        private void OnUICardVisualEndDrag(in OnUICardVisualEndDrag args)
        {
            Vector3 screenPoint = UIRoot.Singleton.UICamera.WorldToScreenPoint(args.visual.transform.position);
            Debug.Log("OnUICardVisualEndDrag: screenPoint " + screenPoint);
            if (RectTransformUtility.RectangleContainsScreenPoint(useCardArea,
                    new Vector2(screenPoint.x, screenPoint.y), UIRoot.Singleton.UICamera))
            {
                Debug.Log("OnUICardVisualEndDrag: use card " + args.data);
                if (GamingMgr.Singleton.PushPlayerOperation(new UseCardOperation(args.data)))
                {
                    cardContainer.RemoveCard(args.data);
                }
            }
        }
    }
}