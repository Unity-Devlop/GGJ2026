using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.孟婆熬药攻击)]
    public class 孟婆熬药攻击 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 孟婆汤中的攻击牌+1。
            return false;
        }
    }
}
