using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [BuffExecutor(BuffEnum.壮魂效果)]
    public class 壮魂效果 : IBuffEffectExecutor
    {
        public void ProcessGainShieldBuffs(IEntityController sender, IEntityController giver, BuffInfo buff, ref int shieldValue)
        {
            
        }

        public void ProcessTakeDamageBuff(IEntityController sender, IEntityController enity, BuffInfo buff,
            ref int damageValue)
        {
        }

        public async UniTask ProcessWhenApplyDamageTo(IEntityController entity, IEntityController tar, int value,
            BuffInfo buff)
        {
            await entity.GainShield((int)buff.parameters);
            // entity.RemoveBuff(buff.buffEnum);
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