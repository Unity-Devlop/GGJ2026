using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.孟婆熬药请神)]
    public class 孟婆熬药请神 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 孟婆汤中的请神牌+1。

          if (atk.TryGetMask(out var mask) && cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
          {
              return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
          }

            return false;
        }
    }
}
