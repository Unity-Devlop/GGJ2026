using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.铜柱鬼叠甲)]
    public class 铜柱鬼叠甲 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 获得{0}点护甲。打出后有{1}%几率结束回合。
            return false;
        }
    }
}
