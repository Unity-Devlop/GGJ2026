namespace GGJ2026.GamePlay
{
    using UnityEngine;
    using DG.Tweening;

    public class CameraShake : MonoBehaviour
    {
        public static CameraShake Instance;

        [Header("默认参数")]
        public float defaultDuration = 0.2f;    // 抖动时间
        public float defaultStrength = 0.5f;    // 抖动力量（幅度）
        public int defaultVibrato = 10;        // 频率（一秒抖动多少次）
        public float defaultRandomness = 90f;  // 随机性

        private Vector3 _originalPos;

        void Awake()
        {
            Instance = this;
            _originalPos = transform.localPosition;
        }

        /// <summary>
        /// 触发相机抖动
        /// </summary>
        /// <param name="customStrength">可选：自定义强度（如暴击时更强）</param>
        public void Shake(float? customStrength = null)
        {
            // 先停止之前的抖动，防止叠加导致位置偏移
            transform.DOKill();
            transform.localPosition = _originalPos;

            float s = customStrength ?? defaultStrength;

            // DOShakePosition 参数：持续时间, 强度, 频率, 随机性, 是否淡出
            transform.DOShakePosition(defaultDuration, s, defaultVibrato, defaultRandomness, false, true)
                .OnComplete(() => transform.localPosition = _originalPos);
        }
    }
}