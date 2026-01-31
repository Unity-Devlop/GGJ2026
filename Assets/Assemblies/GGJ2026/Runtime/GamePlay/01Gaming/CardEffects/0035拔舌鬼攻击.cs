using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.拔舌鬼攻击)]
    public class 拔舌鬼攻击 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 造成{0}点伤害，重复{1}次。打出后有{2}%几率结束回合。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            await atk.UseCard(cardData);
            await tar.TakeCard(cardData);
            for (int i = 0; i < cardData.config.Value[1]; i++)
            {
                await tar.TakeDamage(atk, cardData.config.Value[0]);
            }

            if (Random.Range(0, 100) < cardData.config.Value[2])
            {
                return true;
            }


            return false;
        }
    }
}