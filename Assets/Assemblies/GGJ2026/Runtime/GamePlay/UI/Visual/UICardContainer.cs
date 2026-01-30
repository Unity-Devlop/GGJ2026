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


        private List<UICardSlot> _slots = new();
        private List<UICard> _cards = new();
        private List<UICardVisual> _visuals = new();

        public void Bind(PlayerData playerData)
        {
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