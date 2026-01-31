using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [BuffExecutor(cfg.BuffEnum.禁武令)]
    public class 禁武令 : IBuffEffectExecutor
    {
        public void ProcessGainShieldBuffs(IEntityController sender, IEntityController giver, BuffInfo buff, ref int shieldValue)
        {
            
        }

        public void ProcessTakeDamageBuff(IEntityController sender, IEntityController entity, BuffInfo buff,
            ref int damageValue)
        {
        }

        public async UniTask ProcessWhenApplyDamageTo(IEntityController entity, IEntityController tar, int value,
            BuffInfo buff)
        {
            await entity.TakeDamage(entity, value);
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
        
        public UniTask OnBuffAdded(IEntityController sender, BuffEnum buffEnum, object values, BuffInfo buffInfo)
        {
            sender.RemoveBuff(BuffEnum.卸甲令);
            sender.RemoveBuff(BuffEnum.禁武令);
            return UniTask.CompletedTask;
        }
    }
}