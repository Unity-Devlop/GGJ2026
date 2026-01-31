using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    public interface ICardVisual
    {
        // UICard card { get; }
    }
    
    public interface IOperation
    {
    }

    public interface ICardEffectExecutor
    {
        UniTask Execute(CardData cardData, PlayerController playerController, EnemyController enemyController);
    }
}