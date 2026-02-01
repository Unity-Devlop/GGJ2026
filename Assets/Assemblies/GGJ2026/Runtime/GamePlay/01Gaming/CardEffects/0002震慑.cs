using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.震慑)]
    public class 震慑 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 造成{0}点伤害，有{1}%几率斩杀对手。打出后有{2}%几率结束回合。
            //阎王面具: 造成{0}点伤害，有{1}%几率斩杀对手。打出后有{2}%几率结束回合。
            //无常面具: 造成{0}点伤害，有{1}%几率斩杀对手。打出后有{2}%几率结束回合。有{3}%的概率额外打出一次此牌。
            //阎罗面具: 造成{0}点伤害，有{1}%几率斩杀对手。打出后有{2}%几率结束回合。
            //孟婆面具: 孟婆汤中的攻击牌+1。
            //二郎神面具: 获得{0}点护甲。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            await atk.UseCard(cardData);
            await tar.TakeCard(cardData);
            await tar.TakeDamage(atk, cardData.config.Value[0]);
            bool onceKill = Random.Range(0, 100) >= cardData.config.Value[1];
            if (onceKill)
            {
                await tar.OnceKill();
            }

            return Random.Range(0, 100) < cardData.config.Value[2];
        }
    }
}