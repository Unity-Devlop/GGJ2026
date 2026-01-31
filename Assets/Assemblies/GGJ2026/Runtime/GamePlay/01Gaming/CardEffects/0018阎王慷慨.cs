using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.阎王慷慨)]
    public class 阎王慷慨 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 获得{0}点护甲，然后摸{1}张牌。
            return false;
        }
    }
}
