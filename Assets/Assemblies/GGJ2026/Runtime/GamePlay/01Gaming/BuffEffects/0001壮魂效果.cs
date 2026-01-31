using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [BuffExecutor(BuffEnum.壮魂效果)]
    public class 壮魂效果 : IBuffEffectExecutor
    {
        public void ProcessTakeDamageBuff(IEntityController enity, BuffInfo buff, ref int damageValue)
        {
        }

        public async UniTask ProcessWhenApplyDamageTo(IEntityController entity, IEntityController tar, int value, BuffInfo buff)
        {
            await entity.GainShield((int)buff.parameters);
            entity.RemoveBuff(buff);
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