
using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.阎王万我)]
    public class 阎王万我 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 造成{0}点伤害，重复{1}次。打出后伤害次数永久增加{2}，并且有{3}%几率结束回合。若你造成的伤害次数不小于{4}，受到伤害的敌人死期减少{5}。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            await atk.UseCard(cardData);
            await tar.TakeCard(cardData);

            int damage = cardData.config.Value[0];
            int times = cardData.config.Value[1];
            for (int i = 0; i < times; i++)
            {
                await tar.TakeDamage(atk, damage);
            }

            if (times >= cardData.config.Value[4])
            {
                await tar.ReduceBuff(BuffEnum.死期, cardData.config.Value[5]);
            }


            cardData.config.Value[1] += cardData.config.Value[2];
            return Random.Range(0, 100) < cardData.config.Value[3];
        }
    }
}