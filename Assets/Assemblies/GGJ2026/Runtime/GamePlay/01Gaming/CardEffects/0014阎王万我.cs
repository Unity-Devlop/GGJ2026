using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.阎王万我)]
    public class 阎王万我 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController playerController, IEntityController enemyController)
        {
            // 造成{0}点伤害，重复{1}次。打出后伤害次数永久增加{2}，并且有{3}%几率结束回合。若你造成的伤害次数不小于{4}，受到伤害的敌人死期减少{5}。
            return false;
        }
    }
}
