using System.Collections.Generic;
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
        UniTask TurnStart();
        UniTask TurnEnd();
        int GetUseCardCount(CardEnum cardEnum);
        bool TryGetMask(out MaskEnum id);
        UniTask UseCard(CardData cardData);
        UniTask GainShield(int value);
        UniTask TakeCard(CardData cardData);
        UniTask TakeDamage(int value);
        UniTask OnceKill();
        bool IsDead();
        UniTask GainHealth(int value);
        UniTask DrawCards(int count);
        bool TryGetLastUsedCardThisRound(out CardEnum cardEnum);
        UniTask AddBuff(BuffEnum 碎魂效果, object values);
        void GetBuffs(out List<BuffInfo> buffInfos);
        void RemoveBuff(BuffInfo buff);
        UniTask OnApplyDamageTo(IEntityController tar, int value);
    }

    public interface IBuffEffectExecutor
    {
        void ProcessTakeDamageBuff(IEntityController entityController, BuffInfo buff, ref int damageValue);

        UniTask ProcessWhenApplyDamageTo(IEntityController enemyController, IEntityController tar, int value,
            BuffInfo buff);
    }


    public interface ICardEffectExecutor
    {
        UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar);
    }
}