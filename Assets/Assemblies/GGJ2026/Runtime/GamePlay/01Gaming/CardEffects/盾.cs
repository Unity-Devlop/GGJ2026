using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [CardExecutor(CardEnum.盾)]
    public class 盾 : ICardEffectExecutor
    {
        public async UniTask<bool> Execute(CardData cardData, IEntityController playerController, IEntityController enemyController)
        {
            await playerController.UseCard(cardData);
            await playerController.GainShield(cardData.config.ShieldValue);
            return cardData.config.EndRoundWhenUse;
        }
    }
}