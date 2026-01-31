using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.阎王施加死期)]
    public class 阎王施加死期 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController playerController, IEntityController enemyController)
        {
            // 敌人获得死期效果，倒计时为{0}。已有则无事发生。打出后有{1}%几率结束回合。
            return false;
        }
    }
}
