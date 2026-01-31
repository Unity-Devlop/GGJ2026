using System.Collections.Generic;
using cfg;
using Cysharp.Threading.Tasks;

namespace GGJ2026.GamePlay
{


    public static class CardEffects
    {
        private static Dictionary<CardEnum, ICardEffectExecutor> _cardEffectExecutors = new();

        public static void AutoRegisterAllCardEffects()
        {
            // 通过反射获取所有实现了 ICardEffectExecutor 接口的类
            var executorTypes = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in executorTypes)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(ICardEffectExecutor).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        var attributes = type.GetCustomAttributes(typeof(CardExecutorAttribute), false);
                        if (attributes.Length > 0)
                        {
                            var cardExecutorAttribute = (CardExecutorAttribute)attributes[0];
                            var executorInstance = (ICardEffectExecutor)System.Activator.CreateInstance(type);
                            _cardEffectExecutors[cardExecutorAttribute.cardEnum] = executorInstance;
                        }
                    }
                }
            }
        }
        


        public static async UniTask<bool> ExecuteCardEffects(CardData cardData, IEntityController atk,
            IEntityController target)
        {
           bool result=  await _cardEffectExecutors[cardData.id].Execute(cardData, atk, target);
           return result;
        }
    }
}