using System;
using cfg;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace GGJ2026
{
    public class UICardVisual : MonoBehaviour, ICardVisual
    {
        [SerializeField] private TextMeshProUGUI nameText;
        private RectTransform rectTransform;
        public UICard card { get; private set; }


        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void Bind(UICard uiCard)
        {
            this.card = uiCard;
        }

        private void Update()
        {
            if (card == null) return;
            transform.position = card.transform.position;
        }
    }
}