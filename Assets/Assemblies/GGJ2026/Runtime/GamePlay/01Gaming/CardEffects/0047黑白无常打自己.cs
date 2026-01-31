// c#
using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.黑白无常打自己)]
    public class 黑白无常打自己 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 对自己造成{0}点伤害。打出后有{1}%几率结束回合。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, atk);
            }

            // 自己打出并接牌
            await atk.UseCard(cardData);
            await atk.TakeCard(cardData);

            int damage = cardData.config.Value[0];

            // 自伤前触发钩子（按实际接口调整参数）
            await atk.OnApplyDamageTo(atk, damage);

            // 造成自伤
            await atk.TakeDamage(damage);

            // 按概率结束回合
            return Random.Range(0, 100) < cardData.config.Value[1];
        }
    }
}