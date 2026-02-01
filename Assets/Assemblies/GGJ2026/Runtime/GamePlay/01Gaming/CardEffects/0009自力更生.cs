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
            //无常面具: 视为打出本回合中你上一次打出的牌。打出后有{0}%几率结束回合。
            //孟婆面具: 孟婆汤中的功能牌+1。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            if (atk.TryGetLastUsedCardThisRound(out CardEnum lastCard))
            {
                Debug.Log(lastCard);
                return await CardEffects.ExecuteCardEffects(new CardData(lastCard), atk, tar);
            }
            

            return false;
        }
    }
}