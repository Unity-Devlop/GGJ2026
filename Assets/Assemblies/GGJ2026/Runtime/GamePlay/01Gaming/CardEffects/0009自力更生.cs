using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.自力更生)]
    public class 自力更生 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 视为打出本回合中你上一次打出的牌。打出后有{0}%几率结束回合。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            if (atk.TryGetLastUsedCardThisRound(out CardEnum lastCard))
            {
                Debug.Log(lastCard);
                return await CardEffects.ExecuteCardEffects(new CardData(lastCard), atk, tar);
                //return Random.Range(0, 100) < cardData.config.Value[0];
            }
            

            return false;
        }
    }
}