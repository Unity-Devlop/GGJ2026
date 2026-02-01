
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

            await atk.UseCard(cardData);
            await atk.TakeCard(cardData);

            int damage = cardData.config.Value[0];

            await atk.OnApplyDamageTo(atk, damage);

            await atk.TakeDamage(atk, damage);

            return Random.Range(0, 100) < cardData.config.Value[1];
        }
    }
}