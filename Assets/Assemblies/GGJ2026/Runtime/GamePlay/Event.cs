using cfg;
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
    
    public readonly struct OnUICardVisualBeginDrag
    {
        public readonly UICardVisual visual;
        public readonly CardData data;

        public OnUICardVisualBeginDrag(UICardVisual cardVisual)
        {
            visual = cardVisual;
            data = cardVisual.cardData;
        }
    }
    
    public readonly struct OnUICardVisualDrag
    {
        public readonly UICardVisual visual;
        public readonly CardData data;

        public OnUICardVisualDrag(UICardVisual cardVisual)
        {
            visual = cardVisual;
            data = cardVisual.cardData;
        }
    }
    
    public readonly struct OnUICardVisualPointerEnter
    {
        public readonly CardData data;
        public readonly UICardVisual visual;

        public OnUICardVisualPointerEnter(UICardVisual cardVisual)
        {
            visual = cardVisual;
            data = cardVisual.cardData;
        }
    }

    public readonly struct OnUICardVisualPointerExit
    {
        public readonly CardData data;
        public readonly UICardVisual visual;

        public OnUICardVisualPointerExit(UICardVisual cardVisual)
        {
            visual = cardVisual;
            data = cardVisual.cardData;
        }
    }
    
    
    public readonly struct OnLocalPlayerWearMaskEvent
    {
        public readonly MaskEnum maskID;

        public OnLocalPlayerWearMaskEvent(MaskEnum id)
        {
            maskID = id;
        }
    }
}