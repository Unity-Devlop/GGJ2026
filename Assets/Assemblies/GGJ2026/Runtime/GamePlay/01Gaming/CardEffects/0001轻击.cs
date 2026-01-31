using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.轻击)]
    public class 轻击 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 造成{0}点伤害。
            //阎王面具: 获得{0}点护甲。
            //无常面具: 造成{0}点伤害。
            //孟婆面具: 孟婆汤中的攻击牌+1。
            //二郎神面具: 获得{0}点护甲。
          if (atk.TryGetMask(out var mask) && cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
          {
              return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
          }

          await atk.UseCard(cardData);
          await tar.TakeCard(cardData);
          await tar.TakeDamage(cardData.config.Value[0]);
          await atk.OnApplyDamageTo(tar, cardData.config.Value[0]);
          return false;

            return false;
        }
    }
}
