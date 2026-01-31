using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening; // 引入 DoTween 命名空间

namespace GGJ2026.GamePlay
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DoTweenHitEffect : MonoBehaviour
    {
        [Header("颜色闪烁设置 (Flash Settings)")] [Tooltip("受击时闪烁的目标颜色，通常是纯白色或亮红色")]
        public Color flashColor = Color.white;

        [Tooltip("颜色从闪烁变回原色的持续时间")] public float flashDuration = 0.15f;
        [Tooltip("闪烁次数，设为 2 可能会有更强烈的视觉残留")] public int flashLoops = 1;

        [Header("弹性缩放设置 (Punch Scale Settings)")] [Tooltip("是否启用弹性缩放")]
        public bool enablePunchScale = true;

        [Tooltip("缩放的强度向量。X和Y值越大，形变越夸张。(0.2, 0.2, 0) 是个不错的初始值")]
        public Vector3 punchStrength = new Vector3(0.2f, 0.2f, 0f);

        [Tooltip("缩放效果持续时间")] public float punchDuration = 0.2f;
        [Tooltip("震动频率，数值越大越急促")] public int vibrato = 10;
        [Tooltip("弹性，0-1之间。1表示完全弹回，0表示不弹回。")] public float elasticity = 1f;

        [Header("震动设置 (Shake Position Settings)")] [Tooltip("是否启用位置震动")]
        public bool enableShake = true;

        [Tooltip("震动持续时间")] public float shakeDuration = 0.2f;
        [Tooltip("震动强度，数值越大晃动越剧烈")] public float shakeStrength = 0.3f;

        private SpriteRenderer _spriteRenderer;
        private Color _originalColor;
        private Vector3 _originalScale;
        private bool _isInitializing = true;

        private Tween _colorTween;
        private Tween _scaleTween;
        private Tween _shakeTween;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            // 在 Start 中记录初始状态，确保在任何修改之前
            InitializeState();
        }

        // 确保在物体被启用时状态是正确的
        void OnEnable()
        {
            if (!_isInitializing)
            {
                ResetState();
            }
        }

        private void InitializeState()
        {
            _originalColor = _spriteRenderer.color;
            _originalScale = transform.localScale;
            _isInitializing = false;
        }

        /// <summary>
        /// 供外部调用的核心方法，播放受击效果
        /// </summary>
        [ContextMenu("Test Play Hit Effect")] // 可以在编辑器组件右键菜单中测试
        public async UniTask PlayHitEffect()
        {
            // 1. 重要：在播放新动画前，先杀掉该物体上可能正在运行的旧动画
            // 防止连续受击时动画叠加导致状态错乱（比如卡在白色，或者越变越小）
            KillExistingTweens();

            // 2. 重置状态到初始值，确保每次打击都是从正常状态开始
            ResetState();

            // 3. 执行颜色闪烁效果
            // 创建一个序列：先瞬间变成闪烁色，然后 DoColor 变回原色
            _spriteRenderer.color = flashColor;
            _colorTween = _spriteRenderer.DOColor(_originalColor, flashDuration)
                .SetLoops(flashLoops, LoopType.Yoyo) // Yoyo模式可以让颜色闪过去再闪回来
                .SetEase(Ease.Linear);

            // 4. 执行弹性缩放效果 (PunchScale)
            // PunchScale 会自动处理“变型后再弹回原样”的过程，非常有打击感
            if (enablePunchScale)
            {
                _scaleTween = transform.DOPunchScale(punchStrength, punchDuration, vibrato, elasticity)
                    // 确保动画结束时精确回到原始大小，防止浮点数误差
                    .OnComplete(() => transform.localScale = _originalScale);
            }

            // 5. 执行位置震动效果 (ShakePosition)
            if (enableShake)
            {
                // 注意：只在XY平面震动，不要震动Z轴
                _shakeTween = transform.DOShakePosition(shakeDuration, new Vector3(shakeStrength, shakeStrength, 0),
                    vibrato, 90, false, true);
            }
            
            // 6. 等待所有动画完成
            float maxDuration = Mathf.Max(
                flashDuration * flashLoops,
                enablePunchScale ? punchDuration : 0f,
                enableShake ? shakeDuration : 0f
            );
            await UniTask.Delay(System.TimeSpan.FromSeconds(maxDuration));
        }

        private void KillExistingTweens()
        {
            // 杀死特定的 tween 对象比杀死 transform 上所有的 tween 更安全
            if (_colorTween != null && _colorTween.IsActive()) _colorTween.Kill();
            if (_scaleTween != null && _scaleTween.IsActive()) _scaleTween.Kill();
            if (_shakeTween != null && _shakeTween.IsActive()) _shakeTween.Kill();
        }

        private void ResetState()
        {
            if (_spriteRenderer != null) _spriteRenderer.color = _originalColor;
            transform.localScale = _originalScale;
            // ShakePosition 会自动回到原位，通常不需要手动重置位置，除非你使用了非相对位移
        }

        // 当物体被销毁时进行清理
        void OnDestroy()
        {
            KillExistingTweens();
        }
    }
}