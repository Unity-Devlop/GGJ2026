using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.穷鬼吸血)]
    public class 穷鬼吸血 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 对本我造成{0}点伤害，自己恢复{1}点生命。打出后有{2}%几率结束回合。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            await atk.UseCard(cardData);
            await tar.TakeDamage(cardData.config.Value[0]);
            await atk.OnApplyDamageTo(tar, cardData.config.Value[0]);
            await atk.GainHealth(cardData.config.Value[1]);

            if (Random.Range(0, 100) < cardData.config.Value[2])
            {
                return true;
            }

            return false;
        }
    }
}