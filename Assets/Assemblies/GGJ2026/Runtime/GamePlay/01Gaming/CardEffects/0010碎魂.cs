using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.碎魂)]
    public class 碎魂 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 受到{0}点伤害，然后使你下一次受到的伤害改为{1}。打出后有{2}%几率结束回合。
            //阎王面具: 受到{0}点伤害，然后使你下一次受到的伤害改为{1}。打出后有{2}%几率结束回合。敌人的死期减少{3}。
            //孟婆面具: 孟婆汤中的功能牌+1。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            await atk.UseCard(cardData);
            await atk.TakeCard(cardData);
            await atk.TakeDamage(atk, cardData.config.Value[0]);
            await atk.AddBuff(BuffEnum.碎魂效果, cardData.config.Value[1]);

            return Random.Range(0, 100) < cardData.config.Value[2];
        }
    }
}