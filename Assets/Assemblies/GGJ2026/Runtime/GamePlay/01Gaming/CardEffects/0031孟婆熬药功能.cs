using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.孟婆熬药功能)]
    public class 孟婆熬药功能 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 孟婆汤中的功能牌+1。
            return false;
        }
    }
}
