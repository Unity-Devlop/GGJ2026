using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.慷慨)]
    public class 慷慨 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 受到{0}点伤害，然后摸{1}张牌。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }
            
            await atk.UseCard(cardData);
            await atk.TakeDamage(cardData.config.Value[0]);
            await atk.DrawCards(cardData.config.Value[1]);

            return false;
        }
    }
}