using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.万我)]
    public class 万我 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 造成{0}点伤害，重复{1}次。打出后伤害次数永久增加{2}，并且有{3}%几率结束回合。
            return false;
        }
    }
}
