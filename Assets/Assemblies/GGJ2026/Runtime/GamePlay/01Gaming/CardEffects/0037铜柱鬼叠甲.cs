using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.铜柱鬼叠甲)]
    public class 铜柱鬼叠甲 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 获得{0}点护甲。打出后有{1}%几率结束回合。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            await atk.UseCard(cardData);
            await atk.TakeCard(cardData);
            await atk.GainShield(cardData.config.Value[0]);

            if (Random.Range(0, 100) < cardData.config.Value[1])
            {
                return true;
            }

            return false;
        }
    }
}