using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.孟婆熬药请神)]
    public class 孟婆熬药请神 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 孟婆汤中的请神牌+1。
            return false;
        }
    }
}
