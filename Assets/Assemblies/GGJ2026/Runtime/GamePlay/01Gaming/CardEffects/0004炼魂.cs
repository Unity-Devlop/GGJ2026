using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.炼魂)]
    public class 炼魂 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController playerController, IEntityController enemyController)
        {
            // 获得{0}点护甲。
            await playerController.UseCard(cardData);
            await playerController.GainShield(cardData.config.Value[0]);
            return false;
        }
    }
}
