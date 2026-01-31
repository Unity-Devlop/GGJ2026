using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.阎王减少死期)]
    public class 阎王减少死期 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 敌人的死期减少{0}。打出后有{1}%几率结束回合。
            return false;
        }
    }
}
