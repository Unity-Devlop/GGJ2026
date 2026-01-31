using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.阎王碎魂)]
    public class 阎王碎魂 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController playerController, IEntityController enemyController)
        {
            // 受到{0}点伤害，然后使你下一次受到的伤害改为{1}。打出后有{2}%几率结束回合。敌人的死期减少{3}。
            return false;
        }
    }
}
