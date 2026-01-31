using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.无常吸魂)]
    public class 无常吸魂 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 造成{0}点伤害。若有敌人因此牌的伤害死亡，你恢复{1}点生命。有{2}%的概率额外打出一次此牌。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            await atk.UseCard(cardData);
            await tar.TakeCard(cardData);
            await tar.TakeDamage(atk, cardData.config.Value[0]);
            await atk.OnApplyDamageTo(tar, cardData.config.Value[0]);
            if (tar.IsDead())
            {
                await atk.GainHealth(cardData.config.Value[1]);
            }

            if (Random.Range(0, 100) < cardData.config.Value[2])
            {
                await atk.UseCard(cardData);
                await tar.TakeCard(cardData);
                await tar.TakeDamage(atk, cardData.config.Value[0]);
                await atk.OnApplyDamageTo(tar, cardData.config.Value[0]);
                if (tar.IsDead())
                {
                    await atk.GainHealth(cardData.config.Value[1]);
                }
            }


            return false;
        }
    }
}