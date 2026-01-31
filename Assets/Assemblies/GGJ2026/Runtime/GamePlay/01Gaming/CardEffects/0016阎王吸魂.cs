using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.阎王吸魂)]
    public class 阎王吸魂 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 造成{0}点伤害，获得{1}点护甲。

          if (atk.TryGetMask(out var mask) && cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
          {
              return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
          }
          
            await atk.UseCard(cardData);
            await tar.TakeCard(cardData);
            await tar.TakeDamage(cardData.config.Value[0]);
            await atk.GainShield(cardData.config.Value[1]);
            

            return false;
        }
    }
}
