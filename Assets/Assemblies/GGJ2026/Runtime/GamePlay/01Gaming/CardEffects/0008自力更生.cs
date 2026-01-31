using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.自力更生)]
    public class 自力更生 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController playerController, IEntityController enemyController)
        {
            // 视为打出本回合中你上一次打出的牌。打出后有{0}%几率结束结束回合。
            return false;
        }
    }
}
