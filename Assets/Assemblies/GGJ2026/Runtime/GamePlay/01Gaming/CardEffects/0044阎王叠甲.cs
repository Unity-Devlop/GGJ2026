using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.阎王叠甲)]
    public class 阎王叠甲 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController playerController, IEntityController enemyController)
        {
            // 获得{0}点护甲。打出后有{1}%几率结束回合。
            return false;
        }
    }
}
