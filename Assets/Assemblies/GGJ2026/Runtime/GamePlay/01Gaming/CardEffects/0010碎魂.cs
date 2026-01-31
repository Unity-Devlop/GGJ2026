using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.碎魂)]
    public class 碎魂 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 受到{0}点伤害，然后使你下一次受到的伤害改为{1}。打出后有{2}%几率结束回合。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            await tar.UseCard(cardData);
            await tar.TakeCard(cardData);
            await tar.TakeDamage(atk, cardData.config.Value[0]);
            await tar.AddBuff(BuffEnum.碎魂效果, cardData.config.Value[1]);

            return Random.Range(0, 100) < cardData.config.Value[2];
        }
    }
}