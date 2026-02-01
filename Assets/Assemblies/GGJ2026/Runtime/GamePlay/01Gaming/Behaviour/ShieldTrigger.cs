using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ2026.GamePlay
{
    public class ShieldTrigger : MonoBehaviour
        , IPointerEnterHandler, IPointerExitHandler

    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            Global.Event.Invoke(new OnPointerEnterShieldTriggerEvent());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Global.Event.Invoke(new OnPointerExitShieldTriggerEvent());
        }
    }
}