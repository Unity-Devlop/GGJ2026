using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.阎王碎魂)]
    public class 阎王碎魂 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 受到{0}点伤害，然后使你下一次受到的伤害改为{1}。打出后有{2}%几率结束回合。敌人的死期减少{3}。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            await atk.UseCard(cardData);
            await atk.TakeCard(cardData);
            await atk.TakeDamage(cardData.config.Value[0]);
            await atk.AddBuff(BuffEnum.碎魂效果, cardData.config.Value[1]);
            
            await tar.ReduceBuff(BuffEnum.死期, cardData.config.Value[3]);
            
            if(Random.Range(0, 100) < cardData.config.Value[2])
            {
                return true;
            }

            return false;
        }
    }
}