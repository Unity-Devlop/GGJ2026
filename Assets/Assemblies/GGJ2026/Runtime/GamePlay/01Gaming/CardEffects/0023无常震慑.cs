using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.无常震慑)]
    public class 无常震慑 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController playerController, IEntityController enemyController)
        {
            // 造成{0}点伤害，有{1}%几率斩杀对手。打出后有{2}%几率结束回合。有{3}%的概率额外打出一次此牌。
            return false;
        }
    }
}
