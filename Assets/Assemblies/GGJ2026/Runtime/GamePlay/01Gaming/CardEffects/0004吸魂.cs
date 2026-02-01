using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.吸魂)]
    public class 吸魂 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 造成{0}点伤害。若有敌人因此牌的伤害死亡，你恢复{1}点生命。
            //阎王面具: 造成{0}点伤害，获得{1}点护甲。
            //无常面具: 造成{0}点伤害。若有敌人因此牌的伤害死亡，你恢复{1}点生命。有{2}%的概率额外打出一次此牌。
            //阎罗面具: 造成{0}点伤害。若有敌人因此牌的伤害死亡，你恢复{1}点生命。
            //孟婆面具: 孟婆汤中的攻击牌+1。
            //二郎神面具: 获得{0}点护甲。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            await atk.UseCard(cardData);
            await tar.TakeDamage(atk, cardData.config.Value[0]);

            if (tar.IsDead())
            {
                await atk.GainHealth(cardData.config.Value[1]);
            }

            return false;
        }
    }
}