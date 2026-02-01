using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.万我)]
    public class 万我 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 造成{0}点伤害，重复{1}次。打出后伤害次数永久增加{2}，并且有{3}%几率结束回合。
            //阎王面具: 造成{0}点伤害，重复{1}次。打出后伤害次数永久增加{2}，并且有{3}%几率结束回合。若你造成的伤害次数不小于{4}，受到伤害的敌人死期减少{5}。
            //无常面具: 造成{0}点伤害，重复{1}次，有{2}%的概率使伤害次数增加{3}。打出后伤害次数永久增加{4}，并且有{5}%几率结束回合。
            //孟婆面具: 孟婆汤中的攻击牌+1。
            //二郎神面具: 获得{0}点护甲。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            await atk.UseCard(cardData);
            await tar.TakeCard(cardData);
            for (int i = 0; i < cardData.config.Value[1]; i++)
            {
                await tar.TakeDamage(atk, cardData.config.Value[0]);
                await atk.OnApplyDamageTo(tar, cardData.config.Value[1]);
            }

            cardData.config.Value[1] += cardData.config.Value[2];
            Debug.Log($"万我 damage times increased to {cardData.config.Value[1]}");
            return Random.Range(0, 100) < cardData.config.Value[3];
        }
    }
}