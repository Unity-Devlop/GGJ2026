using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.静心)]
    public class 静心 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 获得{0}点护甲。打出后有{1}%几率结束回合。
            //阎王面具: 获得{0}点护甲。打出后有{1}%几率结束回合。
            //孟婆面具: 孟婆汤中的功能牌+1。

          if (atk.TryGetMask(out var mask) && cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
          {
              return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
          }

            await atk.UseCard(cardData);
            await atk.TakeCard(cardData);
            await atk.GainShield(cardData.config.Value[0]);

            return Random.Range(0, 100) < cardData.config.Value[1];

            return false;
        }
    }
}
