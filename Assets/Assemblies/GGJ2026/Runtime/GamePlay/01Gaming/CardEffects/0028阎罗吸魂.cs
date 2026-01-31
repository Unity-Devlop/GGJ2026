using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.阎罗吸魂)]
    public class 阎罗吸魂 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 造成{0}点伤害。若有敌人因此牌的伤害死亡，你恢复{1}点生命。

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

            return false;
        }
    }
}