using System;
using System.Collections.Generic;
using UnityEngine;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    public class UICardContainer : MonoBehaviour
    {
        [SerializeField] private EasyGameObjectPool slotPool;
        [SerializeField] private EasyGameObjectPool cardPool;
        [SerializeField] private EasyGameObjectPool visualPool;

        [SerializeField] private RectTransform slotRoot;
        [SerializeField] private RectTransform visualRoot;


        [SerializeField] private bool autoSizing = true;
        [SerializeField] public float standardWidth = 100;
        [SerializeField] public float standardHeight = 235;
        [SerializeField] public int standardCount = 8;


        private RectTransform rectTransform;

        private List<UICardSlot> _slots = new();
        private List<UICard> _cards = new();
        private List<UICardVisual> _visuals = new();

        public struct UICardInfo
        {
            public UICardSlot slot;
            public UICard card;
            public UICardVisual visual;
        }

        private Dictionary<CardData, UICardInfo> _cardInfos = new();

        private PlayerData _playerData;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }


        public void AddCard(CardData data)
        {
            var slot = slotPool.Get().GetComponent<UICardSlot>();
            var card = cardPool.Get().GetComponent<UICard>();
            var visual = visualPool.Get().GetComponent<UICardVisual>();
            slot.transform.SetParent(slotRoot, false);
            card.transform.SetParent(slot.transform, false);

            card.Bind(slot, visual);
            visual.Bind(card, data);


            _slots.Add(slot);
            _cards.Add(card);
            _visuals.Add(visual);

            _cardInfos[data] = new UICardInfo
            {
                slot = slot,
                card = card,
                visual = visual
            };
        }

        public void RemoveCard(CardData data)
        {
            var info = _cardInfos[data];

            _slots.Remove(info.slot);
            _cards.Remove(info.card);
            _visuals.Remove(info.visual);

            info.card.UnBind();
            info.visual.UnBind();

            slotPool.Release(info.slot.gameObject);
            cardPool.Release(info.card.gameObject);
            visualPool.Release(info.visual.gameObject);

            _cardInfos.Remove(data);
        }

        public void Bind(PlayerData playerData)
        {
            _playerData = playerData;
            Debug.Log("Bind Card Container");
            foreach (var cardData in playerData.cards)
            {
                AddCard(cardData);
            }
        }

        private void Update()
        {
            if (!autoSizing) return;
            // 根据手牌的数量 动态调整自己的大小
            float maxWidth = standardWidth * standardCount;
            float width = standardWidth * _cards.Count;
            if (width < maxWidth)
            {
                rectTransform.sizeDelta = new Vector2(width, standardHeight);
            }
            else
            {
                rectTransform.sizeDelta = new Vector2(maxWidth, standardHeight);
            }
        }

        [Sirenix.OdinInspector.Button]
        public void UnBind()
        {
            var allCards = new List<CardData>(_cardInfos.Keys);
            foreach (var data in allCards)
            {
                RemoveCard(data);
            }

            _cardInfos.Clear();
            _playerData = null;
        }
    }
}