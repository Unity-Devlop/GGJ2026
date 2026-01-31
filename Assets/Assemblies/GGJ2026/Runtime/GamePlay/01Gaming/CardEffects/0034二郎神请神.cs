using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.二郎神请神)]
    public class 二郎神请神 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 获得{0}点护甲。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }
            
            await atk.UseCard(cardData);
            await atk.TakeCard(cardData);
            await atk.GainShield(cardData.config.Value[0]);

            return false;
        }
    }
}