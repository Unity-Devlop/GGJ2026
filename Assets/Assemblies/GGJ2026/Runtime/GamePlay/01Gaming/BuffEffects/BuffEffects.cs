using System.Collections.Generic;
using System.Threading.Tasks;
using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{
    public static class BuffEffects
    {
        private static Dictionary<BuffEnum, IBuffEffectExecutor> _buffEffectExecutors = new();

        public static void AutoRegisterAllBuffEffects()
        {
            var executorTypes = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in executorTypes)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IBuffEffectExecutor).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        var attributes = type.GetCustomAttributes(typeof(BuffExecutorAttribute), false);
                        if (attributes.Length > 0)
                        {
                            var buffExecutorAttribute = (BuffExecutorAttribute)attributes[0];
                            var executorInstance = (IBuffEffectExecutor)System.Activator.CreateInstance(type);
                            _buffEffectExecutors[buffExecutorAttribute.buffEnum] = executorInstance;
                        }
                    }
                }
            }
        }

        public static void ProcessTakeDamageBuffs(IEntityController entityController, ref int damageValue)
        {
            entityController.GetBuffs(out var buffs);
            foreach (var buff in buffs)
            {
                if (_buffEffectExecutors.TryGetValue(buff.buffEnum, out var executor))
                {
                    executor.ProcessTakeDamageBuff(entityController, buff, ref damageValue);
                }
            }
        }

        public static async UniTask ProcessWhenApplyDamageTo(IEntityController enemyController, IEntityController tar,
            int value)
        {
            enemyController.GetBuffs(out var buffs);
            foreach (var buff in buffs)
            {
                if (_buffEffectExecutors.TryGetValue(buff.buffEnum, out var executor))
                {
                    await executor.ProcessWhenApplyDamageTo(enemyController, tar, value, buff);
                }
            }
        }
    }
}