using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.拔舌鬼攻击)]
    public class 拔舌鬼攻击 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 造成{0}点伤害，重复{1}次。打出后有{2}%几率结束回合。
            return false;
        }
    }
}
