using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.盾)]
    public class 盾 : ICardEffectExecutor
    {
        public async UniTask Execute(CardData cardData, PlayerController playerController, EnemyController enemyController)
        {
            await playerController.UseCard(cardData);
            await playerController.GainShield(cardData.config.ShieldValue);
        }
    }
}