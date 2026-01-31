// c#
using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.阎王空过)]
    public class 阎王空过 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 无事发生。打出后有{1}%几率结束回合。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            await atk.UseCard(cardData);
            await tar.TakeCard(cardData);

            return Random.Range(0, 100) < cardData.config.Value[1];
        }
    }
}