using cfg;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GGJ2026.GamePlay
{
    public class EnemyIntentVisual : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image image;

        private CardTypeEnum _currentIntent;

        // [SerializeField] private TMP_Text intentText;
        public void SetIntent(CardTypeEnum intent)
        {
            image.sprite = Global.refHolder.spriteConfig.cardTypeSprites[intent];
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Global.Event.Invoke(new OnEnemyIntentPointerEnterEvent()
            {
                intent = _currentIntent
            });
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Global.Event.Invoke(new OnEnemyIntentPointerExitEvent
            {
                intent = _currentIntent
            });
        }
    }
}