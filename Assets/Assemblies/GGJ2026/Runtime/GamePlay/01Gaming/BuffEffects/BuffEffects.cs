using System.Collections.Generic;
using System.Threading.Tasks;
using cfg;
using Cysharp.Threading.Tasks;
using UnityToolkit;

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

        public static void ProcessTakeDamageBuffs(IEntityController sender,IEntityController taker, ref int damageValue)
        {
            taker.GetBuffs(out var buffs);
            for (int i = buffs.Count - 1; i >= 0; i--)
            {
                var buff = buffs[i];
                if (_buffEffectExecutors.TryGetValue(buff.buffEnum, out var executor))
                {
                    executor.ProcessTakeDamageBuff(sender,taker, buff, ref damageValue);
                }
            }
        }

        public static async UniTask ProcessWhenApplyDamageTo(IEntityController entity, IEntityController tar,
            int value)
        {
            entity.GetBuffs(out var buffs);
            for (int i = buffs.Count - 1; i >= 0; i--)
            {
                var buff = buffs[i];
                if (_buffEffectExecutors.TryGetValue(buff.buffEnum, out var executor))
                {
                    await executor.ProcessWhenApplyDamageTo(entity, tar, value, buff);
                }
            }
        }

        public static async UniTask OnTurnEnd(IEntityController entity)
        {
            entity.GetBuffs(out var buffs);
            for (int i = buffs.Count - 1; i >= 0; i--)
            {
                var buff = buffs[i];
                if (_buffEffectExecutors.TryGetValue(buff.buffEnum, out var executor))
                {
                    await executor.OnTurnEnd(entity, buff);
                }
            }
        }

        public static async UniTask ReduceBuff(IEntityController entity, BuffEnum id, object parmaters)
        {
            entity.GetBuffs(out var buffs);
            for (int i = buffs.Count - 1; i >= 0; i--)
            {
                var buff = buffs[i];
                if (buff.buffEnum == id)
                {
                    if (_buffEffectExecutors.TryGetValue(buff.buffEnum, out var executor))
                    {
                        await executor.OnReduceBuff(entity, buff, parmaters);
                    }
                }
            }
        }

        public static void ProcessTakeDamageIgnoreShieldBuffs(IEntityController sender,IEntityController taker, ref bool ignoreShield)
        {
            sender.GetBuffs(out var buffs);
            for (int i = buffs.Count - 1; i >= 0; i--)
            {
                var buff = buffs[i];
                if (_buffEffectExecutors.TryGetValue(buff.buffEnum, out var executor))
                {
                    executor.ProcessTakeDamageIgnoreShieldBuffs(sender,taker, buff, ref ignoreShield);
                }
            }
        }
    }
}