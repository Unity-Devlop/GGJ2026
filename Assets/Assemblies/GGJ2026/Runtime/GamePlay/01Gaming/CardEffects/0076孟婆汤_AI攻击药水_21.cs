using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.孟婆汤_AI攻击药水_21)]
    public class 孟婆汤_AI攻击药水_21 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 造成{0}点伤害，摸{1}张牌。

          if (atk.TryGetMask(out var mask) && cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
          {
              return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
          }

            await atk.UseCard(cardData);
            await tar.TakeCard(cardData);
            await tar.TakeDamage(atk, cardData.config.Value[0]);
            await atk.OnApplyDamageTo(tar, cardData.config.Value[0]);
            await atk.DrawCards(cardData.config.Value[1]);
            return false;
        }
    }
}
