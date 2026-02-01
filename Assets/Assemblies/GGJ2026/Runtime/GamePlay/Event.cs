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

    public readonly struct OnMaskButtonPointerEnter
    {
        public readonly MaskEnum maskID;

        public OnMaskButtonPointerEnter(MaskEnum id)
        {
            maskID = id;
        }
    }

    public readonly struct OnMaskButtonPointerExit
    {
        public readonly MaskEnum maskID;

        public OnMaskButtonPointerExit(MaskEnum id)
        {
            maskID = id;
        }
    }

    public readonly struct OnPointerEnterHealthTriggerEvent
    {
    }

    public readonly struct OnPointerExitHealthTriggerEvent
    {
    }

    public readonly struct OnPointerEnterShieldTriggerEvent
    {
    }

    public readonly struct OnPointerExitShieldTriggerEvent
    {
    }

    public readonly struct OnPointerEnterBuffTriggerEvent
    {
        public readonly BuffEnum buffID;

        public OnPointerEnterBuffTriggerEvent(BuffEnum id)
        {
            buffID = id;
        }
    }

    public readonly struct OnPointerExitBuffTriggerEvent
    {
        public readonly BuffEnum buffID;

        public OnPointerExitBuffTriggerEvent(BuffEnum id)
        {
            buffID = id;
        }
    }

    public struct OnPointerEnterPlayerTagEvent
    {
        public IEntityController entityController;
    }

    public struct OnPointerExitPlayerTagEvent
    {
        public IEntityController entityController;
    }

    public struct OnPointerEnterEnemyTagEvent
    {
        public IEntityController entityController;
    }

    public struct OnPointerExitEnemyTagEvent
    {
        public IEntityController entityController;
    }
}