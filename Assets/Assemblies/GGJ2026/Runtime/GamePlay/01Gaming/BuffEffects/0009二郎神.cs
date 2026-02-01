using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [BuffExecutor(BuffEnum.二郎神)]
    public class 二郎神 : IBuffEffectExecutor
    {
        public void ProcessGainShieldBuffs(IEntityController sender, IEntityController giver, BuffInfo buff,
            ref int shieldValue)
        {
        }

        public UniTask ProcessTakeDamageBuff(IEntityController sender, IEntityController entity, BuffInfo buff,
            ref int damageValue)
        {
           return sender.TakeDamage(entity, damageValue);
        }

        public UniTask ProcessWhenApplyDamageTo(IEntityController entity, IEntityController tar, int value,
            BuffInfo buff)
        {
            return UniTask.CompletedTask;
        }

        public UniTask OnTurnEnd(IEntityController entity, BuffInfo buff)
        {
            return UniTask.CompletedTask;
        }

        public UniTask OnReduceBuff(IEntityController entity, BuffInfo buff, object parmaters)
        {
            return UniTask.CompletedTask;
        }

        public void ProcessTakeDamageIgnoreShieldBuffs(IEntityController sender, IEntityController taker, BuffInfo buff,
            ref bool ignoreShield)
        {
        }

        public UniTask OnBuffAdded(IEntityController sender, BuffEnum buffEnum, object values, BuffInfo buffInfo)
        {
            return UniTask.CompletedTask;
        }
    }
}