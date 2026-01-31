using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.无常万我)]
    public class 无常万我 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController playerController, IEntityController enemyController)
        {
            // 造成{0}点伤害，重复{1}次，有{2}%的概率使伤害次数增加{3}。打出后伤害次数永久增加{4}，并且有{5}%几率结束回合。
            return false;
        }
    }
}
