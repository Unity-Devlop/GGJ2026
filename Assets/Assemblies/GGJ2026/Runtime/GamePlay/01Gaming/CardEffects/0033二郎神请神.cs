using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.二郎神请神)]
    public class 二郎神请神 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController playerController, IEntityController enemyController)
        {
            // 获得{0}点护甲。
            return false;
        }
    }
}
