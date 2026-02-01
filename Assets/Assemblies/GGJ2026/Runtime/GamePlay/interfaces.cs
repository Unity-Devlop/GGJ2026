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
        UniTask TakeDamage(IEntityController sender, int value, bool ignoreShield = false);
        UniTask OnceKill();
        bool IsDead();
        UniTask GainHealth(int value);
        UniTask DrawCards(int count);
        bool TryGetLastUsedCardThisRound(out CardEnum cardEnum);
        UniTask AddBuff(BuffEnum buffEnum, object values);
        void GetBuffs(out List<BuffInfo> buffInfos);
        void RemoveBuff(BuffEnum buff);
        UniTask OnApplyDamageTo(IEntityController tar, int value);
        UniTask SwitchMask(MaskEnum id);
        UniTask ReduceBuff(BuffEnum id, object parmaters);
        void AddMengpoData(CardTypeEnum type, int value);
        UniTask ClearShield();
        string GetName();
    }

    public interface IBuffEffectExecutor
    {
        void ProcessGainShieldBuffs(IEntityController sender, IEntityController giver, BuffInfo buff,
            ref int shieldValue);
        UniTask ProcessTakeDamageBuff(IEntityController sender, IEntityController entity, BuffInfo buff,
            ref int damageValue);

        UniTask ProcessWhenApplyDamageTo(IEntityController entity, IEntityController tar, int value,
            BuffInfo buff);

        UniTask OnTurnEnd(IEntityController entity, BuffInfo buff);
        UniTask OnReduceBuff(IEntityController entity, BuffInfo buff, object parmaters);

        void ProcessTakeDamageIgnoreShieldBuffs(IEntityController sender, IEntityController taker, BuffInfo buff,
            ref bool ignoreShield);

        UniTask OnBuffAdded(IEntityController sender, BuffEnum buffEnum, object values, BuffInfo buffInfo);
    }


    public interface ICardEffectExecutor
    {
        UniTask<bool> Execute(CardData cardData, IEntityController atk, IEntityController tar);
    }
}