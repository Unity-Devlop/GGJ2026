using System.Linq;
using cfg;
using Cysharp.Threading.Tasks;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.阎罗傩戏)]
    public class 阎罗傩戏 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 随机切换一个律令（可能重复）。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }

            BuffEnum[] laws =
            {
                BuffEnum.监禁令,
                BuffEnum.卸甲令,
                BuffEnum.禁武令,
            };

            var targetLaw = laws.ToList().RandomTakeWithoutRemove();

            atk.RemoveBuff(BuffEnum.监禁令);
            atk.RemoveBuff(BuffEnum.卸甲令);
            atk.RemoveBuff(BuffEnum.禁武令);

            await atk.AddBuff(targetLaw, null);


            return false;
        }
    }
}