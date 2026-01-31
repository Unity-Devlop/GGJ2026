using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [BuffExecutor(cfg.BuffEnum.白无常)]
    public class 白无常 : IBuffEffectExecutor
    {
        public void ProcessTakeDamageBuff(IEntityController sender, IEntityController enity, BuffInfo buff,
            ref int damageValue)
        {
        }

        public async UniTask ProcessWhenApplyDamageTo(IEntityController entity, IEntityController tar, int value,
            BuffInfo buff)
        {
            await entity.GainHealth(value);
        }

        public UniTask OnTurnEnd(IEntityController entity, BuffInfo buff)
        {
            return UniTask.CompletedTask;
        }

        public UniTask OnReduceBuff(IEntityController entity, BuffInfo buff, object parmaters)
        {
            return UniTask.CompletedTask;
        }

        public void ProcessTakeDamageIgnoreShieldBuffs(IEntityController entity, BuffInfo buff, ref bool ignoreShield)
        {
        }
    }
}