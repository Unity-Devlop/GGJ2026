// c#

using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.阎王减少死期)]
    public class 阎王减少死期 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 敌人的死期减少{0}。打出后有{1}%几率结束回合。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            await atk.UseCard(cardData);
            await tar.TakeCard(cardData);

            // 减少目标的死期（按实际接口调整方法名）
            await tar.ReduceBuff(BuffEnum.死期, cardData.config.Value[0]);

            // 结束回合概率
            return Random.Range(0, 100) < cardData.config.Value[1];
        }
    }
}