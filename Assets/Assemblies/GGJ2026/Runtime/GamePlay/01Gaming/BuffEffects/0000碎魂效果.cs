using cfg;
using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;

namespace GGJ2026.GamePlay
{
    [BuffExecutor(BuffEnum.碎魂效果)]
    public class 碎魂效果 : IBuffEffectExecutor
    {
        public void ProcessTakeDamageBuff(IEntityController entityController, BuffInfo buff, ref int damageValue)
        {
            Assert.IsTrue(buff.buffEnum == BuffEnum.碎魂效果);
            damageValue = 1;
            entityController.RemoveBuff(buff);
        }

        public UniTask ProcessWhenApplyDamageTo(IEntityController enemyController, IEntityController tar, int value, BuffInfo buff)
        {
            return UniTask.CompletedTask;
        }
    }
}