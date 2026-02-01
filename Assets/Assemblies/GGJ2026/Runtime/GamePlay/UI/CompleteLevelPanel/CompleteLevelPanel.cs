using UnityEngine.EventSystems;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    public class CompleteLevelPanel : UIPanel, IPointerClickHandler
    {
        public bool IsConfirmed { get; private set; }

        public override void OnOpened()
        {
            base.OnOpened();
            IsConfirmed = false;
        }

        public override void OnClosed()
        {
            base.OnClosed();
            IsConfirmed = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            IsConfirmed = true;
        }
    }
}