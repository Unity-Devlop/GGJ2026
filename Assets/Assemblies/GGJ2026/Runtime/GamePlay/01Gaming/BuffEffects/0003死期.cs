using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [BuffExecutor(BuffEnum.死期)]
    public class 死期 : IBuffEffectExecutor
    {
        public void ProcessTakeDamageBuff(IEntityController enity, BuffInfo buff, ref int damageValue)
        {
        }

        public UniTask ProcessWhenApplyDamageTo(IEntityController entity, IEntityController tar, int value,
            BuffInfo buff)
        {
            return UniTask.CompletedTask;
        }

        public async UniTask OnTurnEnd(IEntityController entity, BuffInfo buff)
        {
            await OnReduceBuff((EnemyController)entity, buff, -1);
        }

        public async UniTask OnReduceBuff(IEntityController entity, BuffInfo buff, object parmaters)
        {
            buff.parameters = (int)buff.parameters - 1;
            if ((int)buff.parameters <= 0)
            {
                await entity.OnceKill();
                entity.RemoveBuff(buff);
            }
        }
    }
}