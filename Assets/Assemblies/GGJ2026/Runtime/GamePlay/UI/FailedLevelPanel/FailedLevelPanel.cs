using UnityEngine.EventSystems;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    public class FailedLevelPanel : UIPanel, IPointerClickHandler
    {
        public bool isConfirmed;

        public override void OnOpened()
        {
            base.OnOpened();
            isConfirmed = false;
        }

        public override void OnClosed()
        {
            base.OnClosed();
            isConfirmed = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            isConfirmed = true;
        }
    }
}