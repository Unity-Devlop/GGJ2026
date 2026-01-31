using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.阎罗震慑)]
    public class 阎罗震慑 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController playerController, IEntityController enemyController)
        {
            // 造成{0}点伤害，有{1}%的几率斩杀对手。打出后有{2}%的几率结束回合。
            return false;
        }
    }
}
