using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [BuffExecutor(BuffEnum.返回本我效果)]
    public class 傩戏开场效果 : IBuffEffectExecutor
    {
        public void ProcessGainShieldBuffs(IEntityController sender, IEntityController giver, BuffInfo buff, ref int shieldValue)
        {
            
        }


        public UniTask ProcessTakeDamageBuff(IEntityController sender, IEntityController enity, BuffInfo buff,
            ref int damageValue)
        {
            return UniTask.CompletedTask;
        }

        public UniTask ProcessWhenApplyDamageTo(IEntityController entity, IEntityController tar, int value, BuffInfo buff)
        {
            return UniTask.CompletedTask;
        }

        public UniTask OnTurnEnd(IEntityController entity, BuffInfo buff)
        {
            entity.SwitchMask(MaskEnum.本我);
            entity.RemoveBuff(buff.buffEnum);
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