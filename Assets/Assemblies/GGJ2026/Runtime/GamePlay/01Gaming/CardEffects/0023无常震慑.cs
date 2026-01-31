using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.无常震慑)]
    public class 无常震慑 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 造成{0}点伤害，有{1}%几率斩杀对手。打出后有{2}%几率结束回合。有{3}%的概率额外打出一次此牌。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            await atk.UseCard(cardData);
            await tar.TakeCard(cardData);
            await tar.TakeDamage(cardData.config.Value[0]);
            // 斩杀概率
            if (Random.Range(0, 100) < cardData.config.Value[1])
            {
                await tar.OnceKill();
            }

            // 结束回合?
            bool endTurn = Random.Range(0, 100) < cardData.config.Value[2];
            if (endTurn)
            {
                return true;
            }

            // 额外打出一次此牌概率
            if (Random.Range(0, 100) < cardData.config.Value[3])
            {
                return await Execute(cardData, atk, tar);
            }

            return false;
        }
    }
}