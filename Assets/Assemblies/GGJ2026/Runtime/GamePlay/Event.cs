using UnityEngine;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    public readonly struct OnUICardVisualEndDrag
    {
        public readonly UICardVisual visual;
        public readonly CardData data;
            
        public OnUICardVisualEndDrag(UICardVisual cardVisual)
        {
            visual = cardVisual;
            data = cardVisual.cardData;
        }
    }
}