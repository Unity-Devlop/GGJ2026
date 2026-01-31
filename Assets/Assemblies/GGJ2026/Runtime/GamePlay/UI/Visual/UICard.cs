using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    [RequireComponent(typeof(Image))]
    public class UICard : Selectable, IDragHandler, IBeginDragHandler, IEndDragHandler, IPoolObject
    {
        // Events
        public event Action<UICard> PointerEnterEvent = delegate { };
        public event Action<UICard> PointerExitEvent = delegate { };
        public event Action<UICard, bool> PointerUpEvent = delegate { };
        public event Action<UICard> PointerDownEvent = delegate { };
        public event Action<UICard> BeginDragEvent = delegate { };
        public event Action<UICard> EndDragEvent = delegate { };

        public event Action<UICard, bool> SelectEvent = delegate { };

        public event Action<UICard, bool> HoverEvent = delegate { };

        // States
        public bool isDragging { get; private set; }


        public bool isHovering { get; private set; }

        public bool selected { get; private set; }

        /// <summary>
        /// 是否可以自己回到原来的位置
        /// </summary>
        [NonSerialized] public bool canReset = true;

        // Config
        public Vector3 offset;
        public float moveSpeedLimit = 20f;

        public float selectionOffset = 50;
        public float biggerScale = 2f;

        public Vector3 originScale = Vector3.one;

        // components
        public UICardVisual visual { get; private set; }

        // private CardVisualPool _cardVisualPool;
        public Image img { get; private set; }
        private Canvas _canvas;

        public static UICard currentDragCard;


        protected override void Awake()
        {
            base.Awake();
            img = GetComponent<Image>();
            _canvas = GetComponent<Canvas>();
        }

        public void Bind(UICardSlot slot, UICardVisual uiCardVisual)
        {
        }

        public void OnGet()
        {
            gameObject.SetActive(true);
        }

        public void OnRelease()
        {
            originScale = Vector3.one;
            transform.localScale = originScale;
            gameObject.SetActive(false);
            // Reste Events
            PointerEnterEvent = delegate { };
            PointerExitEvent = delegate { };
            PointerUpEvent = delegate { };
            PointerDownEvent = delegate { };
            BeginDragEvent = delegate { };
            EndDragEvent = delegate { };
            SelectEvent = delegate { };
            // Reset States
            isDragging = false;
            isHovering = false;
            selected = false;

            visual = null;
        }

        private void Update()
        {
            if (!Application.isPlaying) return;
            if (isDragging)
            {
                Vector3 mousePosition = Pointer.current.position.value;
                Vector2 targetPosition = UIRoot.Singleton.UICamera.ScreenToWorldPoint(mousePosition) - offset;
                Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
                Vector2 velocity = direction * Mathf.Min(moveSpeedLimit,
                    Vector2.Distance(transform.position, targetPosition) / Time.deltaTime);
                transform.Translate(velocity * Time.deltaTime);

                ClampPosition(); // 限制位置 不能超出屏幕
            }
            else if (canReset)
            {
                RectTransform rectTransform = transform as RectTransform;
                rectTransform.anchoredPosition = Vector2.zero;
            }

            // hovering 但是 drag了一个非自己的时候 不放大
            if (isDragging ||
                (isHovering && currentDragCard == null)
               )
            {
                transform.localScale = originScale * biggerScale;
            }
            else if (canReset)
            {
                transform.localScale = originScale;
            }
        }


        private void ClampPosition()
        {
            Vector2 screenBounds =
                UIRoot.Singleton.UICamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x, screenBounds.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y, screenBounds.y);
            float z = transform.position.z;
            transform.position = new Vector3(clampedPosition.x, clampedPosition.y, z);
            // Debug.Log("ClampPosition, transform.position: " + transform.position);
        }


        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            currentDragCard = this;
            // Debug.Log("OnBeginDrag");
            transform.DOScale(Vector3.one * biggerScale, 0.1f);
            BeginDragEvent(this);
            Vector2 mousePosition = UIRoot.Singleton.UICamera.ScreenToWorldPoint(eventData.position);
            offset = mousePosition - (Vector2)transform.position;
            isDragging = true;
            // Debug.Log("OnBeginDrag, isDragging: " + isDragging);
            _canvas.GetComponent<GraphicRaycaster>().enabled = false;
            img.raycastTarget = false;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
        }

        public virtual async void OnEndDrag(PointerEventData eventData)
        {
            currentDragCard = null;
            EndDragEvent.Invoke(this);
            isDragging = false;
            _canvas.GetComponent<GraphicRaycaster>().enabled = true;
            img.raycastTarget = true;
            await UniTask.Yield();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            PointerEnterEvent.Invoke(this);
            isHovering = true;

            HoverEvent.Invoke(this, isHovering);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            PointerExitEvent.Invoke(this);
            isHovering = false;
            HoverEvent.Invoke(this, isHovering);
        }


        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            PointerDownEvent.Invoke(this);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            PointerUpEvent.Invoke(this, selected);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            // TODO 感觉不好听 不如不要
            // Global.Get<AudioSystem>().PlayOneShot(FMODName.Event.SFX_SFX_UI_选择卡牌);
            selected = true;
            SelectEvent.Invoke(this, selected);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            selected = false;

            SelectEvent.Invoke(this, selected);
            transform.DOKill();
        }


        public int SlotAmount()
        {
            if (transform.parent.TryGetComponent(out UICardSlot slot))
            {
                return slot.transform.parent.childCount - 1;
            }

            return 0;
        }

        public int SlotIndex()
        {
            if (transform.parent.TryGetComponent(out UICardSlot slot))
            {
                return slot.transform.GetSiblingIndex();
            }

            return 0;
        }

        // public float NormalizedPosition()
        // {
        //     if (transform.TryGetComponent(out UICardSlot slot))
        //     {
        //         return CardMathExtensions.Remap(SlotIndex(), 0, SlotAmount(), 0, 1);
        //     }
        //
        //     return 0;
        // }
        public void UnBind()
        {
            
        }
    }
}