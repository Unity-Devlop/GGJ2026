using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    [BuffExecutor(BuffEnum.壮魂效果)]
    public class 壮魂效果 : IBuffEffectExecutor
    {
        public void ProcessTakeDamageBuff(IEntityController entityController, BuffInfo buff, ref int damageValue)
        {
        }

        public async UniTask ProcessWhenApplyDamageTo(IEntityController enemyController, IEntityController tar, int value, BuffInfo buff)
        {
            await enemyController.GainShield((int)buff.parameters);
        }
    }
}