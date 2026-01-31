using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.孟婆熬药攻击)]
    public class 孟婆熬药攻击 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 孟婆汤中的攻击牌+1。

          if (atk.TryGetMask(out var mask) && cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
          {
              return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
          }

            return false;
        }
    }
}
