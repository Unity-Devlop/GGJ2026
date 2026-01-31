using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.慷慨)]
    public class 慷慨 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 受到{0}点伤害，然后摸{1}张牌。
            return false;
        }
    }
}
