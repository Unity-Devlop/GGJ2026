using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ2026.GamePlay
{
    public class HealthTrigger : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
           Global.Event.Invoke<OnPointerEnterHealthTriggerEvent>(new OnPointerEnterHealthTriggerEvent());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Global.Event.Invoke<OnPointerExitHealthTriggerEvent>(new OnPointerExitHealthTriggerEvent());
        }
    }
}