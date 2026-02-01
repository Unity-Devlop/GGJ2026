namespace GGJ2026.GamePlay
{
    using UnityEngine;
    using TMPro;
    using DG.Tweening;
    using Cysharp.Threading.Tasks;

    public class DamagePopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text textMesh;
    
        [Header("动画配置")]
        public float moveDistance = 2f;      // 向上飘的高度
        public float duration = 0.8f;        // 持续总时间
        public float scaleUpStrength = 1.5f; // 初始弹出的缩放倍率
    
        [Header("颜色设置")]
        public Color normalColor = Color.white;
        public Color critColor = Color.red;

        /// <summary>
        /// 初始化并播放飘字动画
        /// </summary>
        /// <param name="damage">伤害数值</param>
        /// <param name="isCritical">是否暴击</param>
        public async UniTask Play(int damage, bool isCritical = false)
        {
            // 1. 初始化状态
            textMesh.text = damage.ToString();
            textMesh.color = isCritical ? critColor : normalColor;
            textMesh.alpha = 1f;
            transform.localScale = Vector3.one * 0.5f; // 初始稍微小一点

            // 2. 创建序列
            Sequence seq = DOTween.Sequence();

            // 向上位移的同时稍微带点左右随机抖动
            Vector3 endPos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), moveDistance, 0);
        
            seq.Append(transform.DOMove(endPos, duration).SetEase(Ease.OutCubic));
        
            // 缩放回弹逻辑：快速变大再恢复常规
            seq.Join(transform.DOScale(isCritical ? scaleUpStrength * 1.2f : scaleUpStrength, duration * 0.2f).SetEase(Ease.OutBack));
            seq.Append(transform.DOScale(1f, duration * 0.1f));

            // 后半段淡出
            seq.Insert(duration * 0.5f, textMesh.DOFade(0, duration * 0.5f));

            // 3. 等待播放完成
            await seq.ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, this.GetCancellationTokenOnDestroy());

            // 4. 回收（根据你的项目，可以是 Destroy 或者放回对象池）
            Destroy(gameObject);
        }
    }
}