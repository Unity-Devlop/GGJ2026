using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.黑白无常叠甲)]
    public class 黑白无常叠甲 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController playerController, IEntityController enemyController)
        {
            // 获得{0}点护甲。打出后有{1}%几率结束回合。
            return false;
        }
    }
}
