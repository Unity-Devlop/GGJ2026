using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.杀)]
    public class 杀 : ICardEffectExecutor
    {

        public async UniTask Execute(CardData cardData, IEntityController playerController, IEntityController enemyController)
        {
            
            await playerController.UseCard(cardData);
            await enemyController.TakeCard(cardData);
            await enemyController.TakeDamage(cardData.config.DamageValue);
        }
    }
}