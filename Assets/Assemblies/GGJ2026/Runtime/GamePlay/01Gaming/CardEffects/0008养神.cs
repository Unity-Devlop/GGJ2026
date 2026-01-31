using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.养神)]
    public class 养神 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController playerController, IEntityController enemyController)
        {
            // 恢复{0}点生命。如果本回合中你打出过此牌的次数不小于{1}，有{2}%几率结束回合。
            return false;
        }
    }
}
