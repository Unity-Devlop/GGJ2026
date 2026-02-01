using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.养神)]
    public class 养神 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 恢复{0}点生命。如果本回合中你打出过此牌的次数不小于{1}，有{2}%几率结束回合。
            //孟婆面具: 孟婆汤中的功能牌+1。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            await atk.UseCard(cardData);
            await atk.GainHealth(cardData.config.Value[0]);
            int count = atk.GetUseCardCount(cardData.id);
            if (count >= cardData.config.Value[1])
            {
                return Random.Range(0, 100) < cardData.config.Value[2];
            }

            return false;
        }
    }
}