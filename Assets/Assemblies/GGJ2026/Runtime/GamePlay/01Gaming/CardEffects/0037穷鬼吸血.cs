using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.穷鬼吸血)]
    public class 穷鬼吸血 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar)
        {
            // 对本我造成{0}点伤害，自己恢复{1}点生命。打出后有{2}%几率结束回合。
            return false;
        }
    }
}
