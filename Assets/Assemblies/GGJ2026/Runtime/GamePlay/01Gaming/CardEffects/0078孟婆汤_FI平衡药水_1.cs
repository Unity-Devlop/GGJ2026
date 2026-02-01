using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.孟婆汤_FI平衡药水_1)]
    public class 孟婆汤_FI平衡药水_1 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 获得{0}点护甲，摸{1}张牌。

          if (atk.TryGetMask(out var mask) && cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
          {
              return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
          }

            await atk.UseCard(cardData);
            await atk.TakeCard(cardData);
            await atk.GainShield(cardData.config.Value[0]);
            await atk.DrawCards(cardData.config.Value[1]);
            return false;
        }
    }
}
