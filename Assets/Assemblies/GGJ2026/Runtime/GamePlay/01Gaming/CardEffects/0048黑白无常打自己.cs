using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.黑白无常打自己)]
    public class 黑白无常打自己 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 对自己造成{0}点伤害。打出后有{1}%几率结束回合。
            return false;
        }
    }
}
