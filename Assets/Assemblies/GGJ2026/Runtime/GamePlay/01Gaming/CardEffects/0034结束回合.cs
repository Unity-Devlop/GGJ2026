using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.结束回合)]
    public class 结束回合 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController playerController, IEntityController enemyController)
        {
            // 结束回合
            return false;
        }
    }
}
