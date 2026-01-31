using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.傩戏开场)]
    public class 傩戏开场 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController playerController, IEntityController enemyController)
        {
            // 直到你的回合结束时，随机佩戴一个面具（从十殿阎罗、孟婆、二郎真君中随机选择）。
            return false;
        }
    }
}
