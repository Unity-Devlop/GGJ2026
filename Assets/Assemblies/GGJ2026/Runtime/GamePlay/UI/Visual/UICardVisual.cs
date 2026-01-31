using System;
using cfg;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ2026.GamePlay
{
    public class UICardVisual : MonoBehaviour, ICardVisual
    {
        [Sirenix.OdinInspector.ShowInInspector]
        public UICard card { get; private set; }


        private float _curveYOffset;
        private float _curveRotationOffset;


        [Header("Follow Parameters")] [SerializeField]
        private float followSpeed = 30;

        [Header("Scale Parameters")] [SerializeField]
        private float scaleSpeed = 20;

        [Header("Rotation Parameters")] [SerializeField]
        private float rotationAmount = 20;

        [SerializeField] private float rotationSpeed = 20;
        // [SerializeField] private float autoTiltAmount = 30;
        // [SerializeField] private float manualTiltAmount = 20;
        // [SerializeField] private float tiltSpeed = 20;

        // [SerializeField] private CurveParameters curve;


        private Canvas _canvas;
        // private Canvas _shadowCanvas;

        [SerializeField] protected Image maskBorder;
        [SerializeField] protected Image background;
        [LabelText("插图")]
        [SerializeField] protected Image mangaImg;
        [LabelText("类型")]
        [SerializeField] protected Image typeImg;
        [SerializeField] protected TextMeshProUGUI nameText;

        [SerializeField] private RectTransform shakeContainer;
        [SerializeField] private Transform tiltContainer;
        // public ActiveSkillTypeEnum id { get; private set; }

        public CardData cardData { get; private set; }


        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        public virtual void Bind(UICard card, CardData cardData)
        {
            Debug.Log("UICardVisual Bind");
            this.cardData = cardData;
            if (this.card != null)
            {
                this.card.PointerEnterEvent -= PointerEnter;
                this.card.PointerExitEvent -= PointerExit;
                this.card.BeginDragEvent -= BeginDrag;
                this.card.EndDragEvent -= EndDrag;
                this.card.PointerDownEvent -= PointerDown;
                this.card.PointerUpEvent -= PointerUp;
                this.card.SelectEvent -= Select;
                this.card.HoverEvent -= Hover;
            }

            this.card = card;
            // id = card.data.config.Type;
            // gameObject.name = card.data.id.ToString();
            card.PointerEnterEvent += PointerEnter;
            card.PointerExitEvent += PointerExit;
            card.BeginDragEvent += BeginDrag;
            card.EndDragEvent += EndDrag;
            card.PointerDownEvent += PointerDown;
            card.PointerUpEvent += PointerUp;
            card.SelectEvent += Select;
            card.HoverEvent += Hover;

            if (Global.refHolder.spriteConfig.cardMangaSprites.TryGetValue(cardData.id, out var sprite))
                mangaImg.sprite = sprite;
            if (Global.refHolder.spriteConfig.cardTypeSprites.TryGetValue(cardData.config.Type, out var typeSprite))
                typeImg.sprite = typeSprite;
            nameText.text = cardData.config.Name;
        }
        private void Start()
        {

        }
        public void UnBind()
        {
            Debug.Log("UICardVisual UnBind");
        }

        private void OnLocalPlayerWearMaskEvent(in OnLocalPlayerWearMaskEvent args)
        {
            // Debug.Log($"OnLocalPlayerWearMaskEvent: maskID={args.maskID}");
            maskBorder.sprite = Global.refHolder.spriteConfig.broaderMaskSprites[args.maskID];
            var atk = GamingMgr.Singleton.GetLocalPlayer();
            string name;
            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                var newCardConfig = Global.tables.CardTable.Get(newCardEffectId);
                name = newCardConfig.Name;
            }
            else
            {
                name = cardData.config.Name;
            }

            nameText.text = name;
        }

        protected virtual void Hover(UICard card, bool hovering)
        {
            if (UICard.currentDragCard != null) return;
            _canvas.overrideSorting = hovering;
        }

        protected virtual void Select(UICard card, bool state)
        {
            _canvas.overrideSorting = state;
            // DOTween.Kill(2, true);
            // float dir = state ? 1 : 0;
            // shakeParent.DOPunchPosition(shakeParent.up * selectPunchAmount * dir, scaleTransition, 10, 1);
            // shakeParent.DOPunchRotation(Vector3.forward * (hoverPunchAngle / 2), hoverTransition, 20, 1).SetId(2);
            //
            // if (scaleAnimations)
            //     transform.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);
        }

        protected virtual void BeginDrag(UICard card)
        {
            // if (scaleAnimations)
            // transform.DOScale(scaleOnSelect, scaleTransition).SetEase(scaleEase);
            Global.Event.Invoke(new OnUICardVisualBeginDrag(this));
            _canvas.overrideSorting = true;
        }

        protected virtual void EndDrag(UICard card)
        {
            Global.Event.Invoke(new OnUICardVisualEndDrag(this));
            _canvas.overrideSorting = false;
            // transform.DOScale(1, scaleTransition).SetEase(scaleEase);

            Global.Event.Invoke(new OnUICardVisualEndDrag(this));
        }


        [Header("Hober Parameters")] [SerializeField]
        private float hoverPunchAngle = 5;

        [SerializeField] private float hoverTransition = .15f;

        protected virtual void PointerEnter(UICard card)
        {
            Global.Event.Invoke(new OnUICardVisualPointerEnter(this));
            DOTween.Kill(2, true);
            shakeContainer.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 1).SetId(2);
            // if (scaleAnimations)
            //     transform.DOScale(scaleOnHover, scaleTransition).SetEase(scaleEase);
            //
            // DOTween.Kill(2, true);
            // shakeParent.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverTransition, 20, 1).SetId(2);
        }

        protected virtual void PointerExit(UICard card)
        {
            Global.Event.Invoke(new OnUICardVisualPointerExit(this));
            // if (!card.wasDragged)
            // transform.DOScale(1, scaleTransition).SetEase(scaleEase);
        }

        protected virtual void PointerUp(UICard card, bool longPress)
        {
            // if (scaleAnimations)
            // transform.DOScale(longPress ? scaleOnHover : scaleOnSelect, scaleTransition).SetEase(scaleEase);
            _canvas.overrideSorting = false;

            // visualShadow.localPosition = shadowDistance;
            // _shadowCanvas.overrideSorting = true;
        }

        protected virtual void PointerDown(UICard card)
        {
            // if (scaleAnimations)
            // transform.DOScale(scaleOnSelect, scaleTransition).SetEase(scaleEase);

            // visualShadow.localPosition += (-Vector3.up * shadowOffset);
            // _shadowCanvas.overrideSorting = false;
        }

        protected virtual void Update()
        {
            typeImg.SetNativeSize();
            mangaImg.SetNativeSize();
            if (card == null)
            {
                return;
            }
      
            var atk = GamingMgr.Singleton.GetLocalPlayer();
            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                OnLocalPlayerWearMaskEvent(new OnLocalPlayerWearMaskEvent(mask));
            }
            else
            {
                OnLocalPlayerWearMaskEvent(new OnLocalPlayerWearMaskEvent(MaskEnum.本我));
            }

            if (card.isDragging)
            {
                _canvas.overrideSorting = true;
            }

            if (card.isHovering && UICard.currentDragCard == null)
            {
                _canvas.overrideSorting = true;
            }

            gameObject.SetActive(card.gameObject.activeInHierarchy);
            SmoothScale();
            // HandPositioning();
            SmoothFollow();
            FollowRotation();
            // CardTilt();
        }

        private void SmoothScale()
        {
            Vector3 targetScale = card.transform.localScale;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
        }

        public virtual void Dispose()
        {
        }

        // [SerializeField] private CardVisualParameters curve;


        /// <summary>
        /// 随时间自动旋转
        /// </summary>
        // protected virtual void HandPositioning()
        // {
        //     _curveYOffset = (curve.positioning.Evaluate(_target.NormalizedPosition()) * curve.positioningInfluence) *
        //                     _target.SlotAmount();
        //     _curveYOffset = _target.SlotAmount() < 5 ? 0 : _curveYOffset;
        //     _curveRotationOffset = curve.rotation.Evaluate(_target.NormalizedPosition());
        // }
        protected virtual void SmoothFollow()
        {
            Vector3 verticalOffset = (Vector3.up * (card.isDragging ? 0 : _curveYOffset));
            Vector3 target = card.transform.position + verticalOffset;

            // // 加上扇形的偏移
            // int amount = _target.SlotAmount();
            // if (amount != 0)
            // {
            //     float percent = _target.SlotIndex() / (float)amount;
            //     float angle = percent * 90 - 45;
            //     float x = Mathf.Sin(angle * Mathf.Deg2Rad);
            //     // x = curve.positioning.Evaluate(percent) * curve.positioningInfluence * x;
            //     float y = Mathf.Cos(angle * Mathf.Deg2Rad);
            //     // y = curve.positioning.Evaluate(percent) * curve.positioningInfluence * y;
            //     target += new Vector3(x, y, 0) / amount * curve.positioningRadius;
            // }
            // else
            // {
            //     // Debug.LogWarning($"{_target}.SlotAmount is 0");
            // }

            transform.position = Vector3.Lerp(transform.position, target,
                followSpeed * Time.deltaTime);
        }

        private Vector3 _followRotationMovementDelta;
        private Vector3 _rotationDelta;
        [SerializeField] private float followRotationSpeed = 25;
        [SerializeField] private Vector2 rotationLimits = new Vector2(-60, 60);

        protected virtual void FollowRotation()
        {
            Vector3 movement = (transform.position - card.transform.position);
            _followRotationMovementDelta = Vector3.Lerp(_followRotationMovementDelta, movement,
                followRotationSpeed * Time.deltaTime);
            Vector3 movementRotation = (card.isDragging ? _followRotationMovementDelta : movement) * rotationAmount;
            _rotationDelta = Vector3.Lerp(_rotationDelta, movementRotation, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
                Mathf.Clamp(_rotationDelta.x, rotationLimits.x, rotationLimits.y));
        }

        [SerializeField] private float autoTiltAmount = 30;
        [SerializeField] private float manualTiltAmount = 20;
        [SerializeField] private float tiltSpeed = 20;

        private int _savedIndex;

        //
        // protected virtual void CardTilt()
        // {
        //     _savedIndex = _target.isDragging ? _savedIndex : _target.SlotIndex();
        //     float sine = Mathf.Sin(Time.time + _savedIndex) * (_target.isHovering ? .2f : 1);
        //     float cosine = Mathf.Cos(Time.time + _savedIndex) * (_target.isHovering ? .2f : 1);
        //
        //     Vector3 offset = transform.position - Global.cameraSystem.mainCamera.ScreenToWorldPoint(Pointer.current.position.value);
        //     float tiltX = _target.isHovering ? offset.y * -1 * manualTiltAmount : 0;
        //     float tiltY = _target.isHovering ? offset.x * manualTiltAmount : 0;
        //
        //     if (offset.y < 0)
        //     {
        //         tiltX = -tiltX;
        //         // tiltY = -tiltY;
        //     }
        //
        //
        //     float tiltZ = _target.isDragging
        //         ? tiltContainer.eulerAngles.z
        //         : (_curveRotationOffset * (curve.rotationInfluence * _target.SlotAmount()));
        //
        //     float lerpX = Mathf.LerpAngle(tiltContainer.eulerAngles.x, tiltX + (sine * autoTiltAmount),
        //         tiltSpeed * Time.deltaTime);
        //     float lerpY = Mathf.LerpAngle(tiltContainer.eulerAngles.y, tiltY + (cosine * autoTiltAmount),
        //         tiltSpeed * Time.deltaTime);
        //     float lerpZ = Mathf.LerpAngle(tiltContainer.eulerAngles.z, tiltZ, tiltSpeed / 2 * Time.deltaTime);
        //
        //     tiltContainer.eulerAngles = new Vector3(lerpX, lerpY, lerpZ);
        // }

        // public void SetParameters(CardVisualParameters visualParameters)
        // {
        //     this.curve = visualParameters;
        // }
    }
}