using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [BuffExecutor(BuffEnum.返回本我效果)]
    public class 傩戏开场效果 : IBuffEffectExecutor
    {
        public void ProcessTakeDamageBuff(IEntityController enity, BuffInfo buff, ref int damageValue)
        {
            
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
    }
}