using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.阎罗吸魂)]
    public class 阎罗吸魂 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController playerController, IEntityController enemyController)
        {
            // 造成{0}点伤害。若有敌人因此牌的伤害死亡，你恢复{1}点生命。
            return false;
        }
    }
}
