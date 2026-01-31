using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.轻击)]
    public class 轻击 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController playerController,
            IEntityController enemyController)
        {
            await playerController.UseCard(cardData);
            await enemyController.TakeCard(cardData);
            await enemyController.TakeDamage(cardData.config.Value[0]);
            return false;
        }
    }
}