using System.Threading.Tasks;
using cfg;
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

    public interface IEntityController
    {
        bool TryGetMask(out MaskEnum id);
        UniTask UseCard(CardData cardData);
        UniTask GainShield(int value);
        UniTask TakeCard(CardData cardData);
        UniTask TakeDamage(int value);
        UniTask OnceKill();
        bool IsDead();
        UniTask GainHealth(int value);
        UniTask DrawCards(int count);
    }

    public interface ICardEffectExecutor
    {
        UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar);
    }
}