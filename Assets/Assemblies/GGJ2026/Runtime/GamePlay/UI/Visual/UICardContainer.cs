using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityToolkit;

namespace GGJ2026
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

        private PlayerData _playerData;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void Bind(PlayerData playerData)
        {
            _playerData = playerData;
            Debug.Log("Bind Card Container");
            Assert.IsTrue(_slots.Count == 0);
            Assert.IsTrue(_cards.Count == 0);
            Assert.IsTrue(_visuals.Count == 0);
            foreach (var skillData in playerData.cards)
            {
                var slot = slotPool.Get().GetComponent<UICardSlot>();
                var card = cardPool.Get().GetComponent<UICard>();
                var visual = visualPool.Get().GetComponent<UICardVisual>();
                slot.transform.SetParent(slotRoot, false);
                card.transform.SetParent(slot.transform, false);

                card.Bind(slot, visual);
                visual.Bind(card);


                _slots.Add(slot);
                _cards.Add(card);
                _visuals.Add(visual);
            }
        }

        private void Update()
        {
            if (!autoSizing) return;
            // 根据手牌的数量 动态调整自己的大小
            float maxWidth = standardWidth * standardCount;
            float width = standardWidth * _playerData.cards.Count;
            if (width < maxWidth)
            {
                rectTransform.sizeDelta = new Vector2(width, standardHeight);
            }
            else
            {
                rectTransform.sizeDelta = new Vector2(maxWidth, standardHeight);
            }
        }

        public void UnBind()
        {
            foreach (var slot in _slots)
            {
                slotPool.Release(slot.gameObject);
            }

            _slots.Clear();

            foreach (var card in _cards)
            {
                cardPool.Release(card.gameObject);
            }

            _cards.Clear();

            foreach (var visual in _visuals)
            {
                visualPool.Release(visual.gameObject);
            }

            _visuals.Clear();
        }
    }
}