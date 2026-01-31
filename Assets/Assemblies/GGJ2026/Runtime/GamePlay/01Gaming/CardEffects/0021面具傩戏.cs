using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.面具傩戏)]
    public class 面具傩戏 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 此牌无效。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            return false;
        }
    }
}