namespace GGJ2026.GamePlay
{
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;
    using Cysharp.Threading.Tasks;

    [RequireComponent(typeof(CanvasGroup))]
    public class EnemyTurnUIEffect : MonoBehaviour
    {
        [Header("组件引用")] [SerializeField] private RectTransform container; // 整个 UI 的容器
        [SerializeField] private CanvasGroup canvasGroup; // 透明度控制
        [SerializeField] private Image bgOverlay; // 全屏半透明黑色遮罩

        [Header("动画参数")] public float punchDuration = 0.4f; // 撞击持续时间
        public float stayDuration = 0.8f; // 停留时间
        public float exitDuration = 0.3f; // 消失时间

        [Header("视觉反馈")] public float shakeStrength = 10f; // 撞击时的屏幕震动强度
        public Color bgFadeColor = new Color(0, 0, 0, 0.6f); // 遮罩颜色

        private bool _awakened = false;

        void Awake()
        {
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            if (bgOverlay != null) bgOverlay.color = Color.clear;
            gameObject.SetActive(false);
            _awakened = true;
        }

        [Sirenix.OdinInspector.Button]
        public async UniTask PlayEffectAsync()
        {
            if (!_awakened) Awake();
            gameObject.SetActive(true);
            var ct = this.GetCancellationTokenOnDestroy();

            // 1. 准备阶段
            container.localScale = Vector3.one * 3f; // 初始倍率很大，准备撞击
            canvasGroup.alpha = 0;

            // 2. 创建序列
            Sequence enemySeq = DOTween.Sequence();

            // --- 入场：急速缩小撞击 ---
            enemySeq.Append(canvasGroup.DOFade(1, punchDuration * 0.5f));
            enemySeq.Join(container.DOScale(1f, punchDuration).SetEase(Ease.InExpo)); // 极快进入

            if (bgOverlay != null)
                enemySeq.Join(bgOverlay.DOColor(bgFadeColor, punchDuration));

            // --- 撞击瞬间：屏幕震动 ---
            enemySeq.AppendCallback(() =>
            {
                // 给整个容器一个 Punch 效果，产生撞击颤动感
                container.DOPunchPosition(new Vector3(0, -10f, 0), 0.3f, 15, 0.5f);
                // 如果想晃动相机，可以用 Camera.main.transform.DOShakePosition
            });

            // --- 停留 ---
            enemySeq.AppendInterval(stayDuration);

            // --- 出场：向两侧撕裂或简单淡出 ---
            enemySeq.Append(container.DOScale(new Vector3(1.5f, 0.1f, 1f), exitDuration)
                .SetEase(Ease.InCubic)); // 横向压扁消失
            enemySeq.Join(canvasGroup.DOFade(0, exitDuration));

            if (bgOverlay != null)
                enemySeq.Join(bgOverlay.DOColor(Color.clear, exitDuration));

            // 等待完成
            await enemySeq.ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, ct);

            gameObject.SetActive(false);
        }
    }
}