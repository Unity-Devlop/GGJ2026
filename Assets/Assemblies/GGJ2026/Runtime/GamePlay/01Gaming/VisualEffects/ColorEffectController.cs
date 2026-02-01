namespace GGJ2026.GamePlay
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal; // 必须引用 URP
    using DG.Tweening;

    public class ColorEffectController : MonoBehaviour
    {
        public static ColorEffectController Instance;

        private Volume _volume;
        private ColorAdjustments _colorAdjustments;

        void Awake()
        {
            Instance = this;
            _volume = GetComponent<Volume>();
        
            // 从 Profile 中获取 ColorAdjustments 缓存
            if (_volume.profile.TryGet(out ColorAdjustments ca))
            {
                _colorAdjustments = ca;
            }
        }

        /// <summary>
        /// 开启“大招模式”色彩：对比度拉满、饱和度降至黑白
        /// </summary>
        public void PlayUltimateColor(float duration = 0.2f)
        {
            // 确保选项已勾选（Override）
            _colorAdjustments.saturation.overrideState = true;
            _colorAdjustments.contrast.overrideState = true;

            // 使用 DOTween 进行插值
            DOTween.To(() => _colorAdjustments.saturation.value, x => _colorAdjustments.saturation.value = x, -21f, duration);
        }

        /// <summary>
        /// 恢复正常色彩
        /// </summary>
        public void ResetColor(float duration = 0.5f)
        {
            DOTween.To(() => _colorAdjustments.saturation.value, x => _colorAdjustments.saturation.value = x, 0f, duration);
        }
    }
}