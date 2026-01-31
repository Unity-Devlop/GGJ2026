using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.无常万我)]
    public class 无常万我 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 造成{0}点伤害，重复{1}次，有{2}%的概率使伤害次数增加{3}。打出后伤害次数永久增加{4}，并且有{5}%几率结束回合。

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
                await atk.OnApplyDamageTo(tar, cardData.config.Value[0]);
            }

            // 有概率增加伤害次数
            if (UnityEngine.Random.Range(0, 100) < cardData.config.Value[2])
            {
                cardData.config.Value[1] += cardData.config.Value[3];
            }

            // 永久增加伤害次数
            cardData.config.Value[1] += cardData.config.Value[4];
            // 结束回合概率
            if (UnityEngine.Random.Range(0, 100) < cardData.config.Value[5])
            {
                return true;
            }


            return false;
        }
    }
}