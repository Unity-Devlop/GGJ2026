using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.无常吸魂)]
    public class 无常吸魂 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 造成{0}点伤害。若有敌人因此牌的伤害死亡，你恢复{1}点生命。有{2}%的概率额外打出一次此牌。
            return false;
        }
    }
}
