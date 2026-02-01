using System.Linq;
using cfg;
using Cysharp.Threading.Tasks;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.傩戏开场)]
    public class 傩戏开场 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 直到你的回合结束时，随机佩戴一个面具（从十殿阎罗、孟婆、二郎真君中随机选择）。
            //阎王面具: 此牌无效。
            //无常面具: 此牌无效。
            //阎罗面具: 随机切换一个律令（可能重复）。
            //孟婆面具: 孟婆汤中的请神牌+1。
            //二郎神面具: 获得{0}点护甲。

            if (atk.TryGetMask(out var mask) &&
                cardData.config.MaskToCardEffect.TryGetValue(mask, out var newCardEffectId))
            {
                return await CardEffects.ExecuteCardEffects(new CardData(newCardEffectId), atk, tar);
            }
            
            await atk.UseCard(cardData);
            await atk.TakeCard(cardData);
            
            MaskEnum[] possibleMasks = new MaskEnum[]
            {
                MaskEnum.阎罗面具,
                MaskEnum.孟婆面具,
                MaskEnum.二郎神面具
            };

            var target = possibleMasks.ToList().RandomTakeWithoutRemove();
            await atk.SwitchMask(target);
            await atk.AddBuff(BuffEnum.返回本我效果,null);

            return false;
        }
    }
}