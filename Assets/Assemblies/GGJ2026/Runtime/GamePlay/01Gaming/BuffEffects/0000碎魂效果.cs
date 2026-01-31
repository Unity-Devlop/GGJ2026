using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;

namespace GGJ2026.GamePlay
{
    [BuffExecutor(BuffEnum.碎魂效果)]
    public class 碎魂效果 : IBuffEffectExecutor
    {
        public void ProcessGainShieldBuffs(IEntityController sender, IEntityController giver, BuffInfo buff, ref int shieldValue)
        {
            
        }

        public void ProcessTakeDamageBuff(IEntityController sender, IEntityController entity, BuffInfo buff,
            ref int damageValue)
        {
            Assert.IsTrue(buff.buffEnum == BuffEnum.碎魂效果);
            damageValue = 1;
            entity.RemoveBuff(buff.buffEnum);
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
    }
}