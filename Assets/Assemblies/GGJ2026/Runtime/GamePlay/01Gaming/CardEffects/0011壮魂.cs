using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.壮魂)]
    public class 壮魂 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 直到你的下个回合结束时，当你造成伤害时，你获得{0}点护甲。
            //孟婆面具: 孟婆汤中的请神牌+1。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }
            
            await atk.UseCard(cardData);
            await atk.TakeCard(cardData);
            await atk.AddBuff(BuffEnum.壮魂效果, cardData.config.Value[0]);

            return false;
        }
    }
}