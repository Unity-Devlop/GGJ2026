using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [BuffExecutor(cfg.BuffEnum.黑无常)]
    public class 黑无常 : IBuffEffectExecutor
    {

        public void ProcessTakeDamageBuff(IEntityController sender, IEntityController entity, BuffInfo buff, ref int damageValue)
        {
        }

        public UniTask ProcessWhenApplyDamageTo(IEntityController entity, IEntityController tar, int value, BuffInfo buff)
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
            ignoreShield = true;
        }
    }
}