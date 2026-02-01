namespace GGJ2026.GamePlay
{
    using UnityEngine;
    using DG.Tweening;
    using Cysharp.Threading.Tasks;

    public static class AttackTweenExtensions
    {
        /// <summary>
        /// 播放攻击动画
        /// </summary>
        /// <param name="target">攻击者Transform</param>
        /// <param name="isRight">方向：true向右，false向左</param>
        /// <param name="strength">冲撞距离</param>
        public static async UniTask PlayAttackAnimation(this Transform target, bool isRight, float strength = 2f)
        {
            // 计算方向系数
            float dir = isRight ? 1f : -1f;
            Vector3 originalPos = target.localPosition;

            // 创建序列
            Sequence seq = DOTween.Sequence();

            // 1. 蓄力阶段：向后微微拉动，准备冲刺
            seq.Append(target.DOLocalMoveX(originalPos.x - (0.5f * dir), 0.15f).SetEase(Ease.OutQuad));

            // 2. 冲撞阶段：快速撞向目标
            // 使用 Ease.InBack 或 Ease.InQuint 增加爆发感
            seq.Append(target.DOLocalMoveX(originalPos.x + (strength * dir), 0.1f).SetEase(Ease.InCubic));

            // 3. 停顿感（命中瞬间的静止）
            seq.AppendInterval(0.05f);

            // 4. 归位阶段：带一点弹性地回到原位
            seq.Append(target.DOLocalMove(originalPos, 0.3f).SetEase(Ease.OutBack));

            // 5. 等待动画完成
            await seq.Play().ToUniTask();
        }

        /// <summary>
        /// 受到攻击时的“受击晃动”
        /// </summary>
        public static void PlayHurtShake(this Transform target)
        {
            // 随机方向的剧烈抖动
            target.DOShakePosition(0.2f, 0.3f, 15, 90, false, true);
        }
    }
}