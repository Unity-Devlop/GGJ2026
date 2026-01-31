using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.壮魂)]
    public class 壮魂 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 直到你的下个回合结束时，当你造成伤害时，你获得{0}点护甲。
            return false;
        }
    }
}
