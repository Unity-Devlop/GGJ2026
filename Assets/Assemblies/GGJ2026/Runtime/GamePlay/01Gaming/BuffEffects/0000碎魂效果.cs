using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;

namespace GGJ2026.GamePlay
{
    [BuffExecutor(BuffEnum.碎魂效果)]
    public class 碎魂效果 : IBuffEffectExecutor
    {
        public void ProcessTakeDamageBuff(IEntityController enity, BuffInfo buff, ref int damageValue)
        {
            Assert.IsTrue(buff.buffEnum == BuffEnum.碎魂效果);
            damageValue = 1;
            enity.RemoveBuff(buff);
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
    }
}