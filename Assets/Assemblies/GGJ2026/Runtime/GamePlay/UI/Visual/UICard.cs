using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.InputSystem;
using UnityToolkit;

namespace GGJ2026
{
    // Slot不会改变位置
    // Card会改变位置
    // Visual会跟着Card改变位置

    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class UICard : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
        IEndDragHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler , IDeselectHandler
    {
        private CanvasGroup _canvasGroup;
        public bool isHovering;
        public RectTransform rectTransform { get; private set; }
        public UICardSlot slot { get; private set; }
        public ICardVisual visual { get; private set; }
        // public float moveSpeedLimit = 20f; // 每秒移动的最大速度

        // 
        public bool isDragging { get; private set; }
        public event Action<UICard> BeginDragEvent;
        public event Action<UICard> EndDragEvent;
        public event Action<UICard> PointerDownEvent;
        public event Action<UICard, bool> PointerUpEvent;
        public event Action<UICard, bool> SelectEvent;
        public event Action<UICard, bool> HoverEvent;
        public event Action<UICard> PointerEnterEvent;
        public event Action<UICard> PointerExitEvent;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Bind(UICardSlot uiCardSlot, ICardVisual uiCardSkillVisual)
        {
            slot = uiCardSlot;
            visual = uiCardSkillVisual;
        }

        public void UnBind()
        {
            slot = null;
            visual = null;
        }

        private void Update()
        {
            // TODO 必须在自己的回合才可以拖拽移动
            if (isDragging)
            {
                Vector3 mousePos = Mouse.current.position.ReadValue();
                Vector2 targetPos = UIRoot.Singleton.UICamera.ScreenToWorldPoint(mousePos);
                // TODO 做平滑跟随移动效果
                // Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
                // float distance = Vector2.Distance(transform.position, targetPos);
                // Vector2 velocity = direction * Mathf.Min(moveSpeedLimit, distance / Time.deltaTime);
                // transform.Translate(velocity * Time.deltaTime);
                transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
                ClampPosition(); // 限制位置 不能超出屏幕
            }
            else
            {
                rectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ClampPosition()
        {
            Vector2 screenBounds =
                UIRoot.Singleton.UICamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x, screenBounds.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y, screenBounds.y);
            float z = transform.position.z;
            transform.position = new Vector3(clampedPosition.x, clampedPosition.y, z);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            BeginDragEvent?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
            EndDragEvent?.Invoke(this);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovering = true;
            PointerEnterEvent?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovering = false;
            PointerExitEvent?.Invoke(this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PointerDownEvent?.Invoke(this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerUpEvent?.Invoke(this, isHovering);
        }

        public void OnSelect(BaseEventData eventData)
        {
            SelectEvent?.Invoke(this, true);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            SelectEvent?.Invoke(this, false);
        }
    }
}