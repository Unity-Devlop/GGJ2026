using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening; // 确保导入 DOTween
    using TMPro; // 如果你使用的是 TextMeshPro

    [RequireComponent(typeof(CanvasGroup))]
    public class TurnStartUIEffect : MonoBehaviour
    {
        [Header("组件引用")] [SerializeField] private RectTransform mainTextRect; // 提示文字的 RectTransform
        [SerializeField] private CanvasGroup canvasGroup; // 用于控制整体透明度
        [SerializeField] private Image backgroundBar; // 可选：背景装饰底条

        [Header("动画参数")] public float enterDuration = 0.5f; // 进入持续时间
        public float stayDuration = 1.0f; // 停留展示时间
        public float exitDuration = 0.3f; // 退出持续时间
        public float startOffset = 500f; // 初始位移偏量（从多远处滑入）

        private Vector2 _originalPos;
        private Vector3 _originalScale;

        void Awake()
        {
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
            _originalPos = mainTextRect.anchoredPosition;
            _originalScale = mainTextRect.localScale;

            // 初始状态：透明且隐藏
            canvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }

        [Sirenix.OdinInspector.Button]
        public async UniTask PlayEffect(System.Action onComplete = null)
        {
            // 1. 初始化状态
            gameObject.SetActive(true);
            canvasGroup.alpha = 0;
            mainTextRect.anchoredPosition = new Vector2(_originalPos.x - startOffset, _originalPos.y);
            mainTextRect.localScale = _originalScale * 0.8f; // 初始稍微小一点

            if (backgroundBar != null) backgroundBar.fillAmount = 0;

            // 2. 创建动画序列 (Sequence)
            Sequence turnSequence = DOTween.Sequence();

            // --- 入场阶段 ---
            turnSequence.Append(canvasGroup.DOFade(1, enterDuration * 0.5f)); // 透明度淡入
            turnSequence.Join(mainTextRect.DOAnchorPos(_originalPos, enterDuration).SetEase(Ease.OutBack)); // 滑入并带回弹
            turnSequence.Join(mainTextRect.DOScale(_originalScale, enterDuration).SetEase(Ease.OutBack)); // 缩放恢复

            if (backgroundBar != null)
            {
                turnSequence.Join(backgroundBar.DOFillAmount(1, enterDuration).SetEase(Ease.OutCubic));
            }

            // --- 停留阶段 ---
            turnSequence.AppendInterval(stayDuration);

            // --- 出场阶段 ---
            // 向上方滑出同时淡出
            turnSequence.Append(mainTextRect
                .DOAnchorPos(new Vector2(_originalPos.x + startOffset * 0.5f, _originalPos.y), exitDuration)
                .SetEase(Ease.InCubic));
            turnSequence.Join(canvasGroup.DOFade(0, exitDuration));

            // --- 结束回调 ---
            turnSequence.OnComplete(() =>
            {
                gameObject.SetActive(false);
                onComplete?.Invoke(); // 执行回调逻辑，比如正式开启玩家操作
            });

            await turnSequence;
        }
    }
}