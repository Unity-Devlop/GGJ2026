using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.阎罗傩戏)]
    public class 阎罗傩戏 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 随机切换一个律令（可能重复）。
            return false;
        }
    }
}
