using System;
using UnityEngine;
using UnityToolkit;

namespace Jump
{
    public class GameFlow :
        MonoBehaviour,
        IOnInit,
        IOnUpdate,
        ISystem
    {
        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        public StateMachine<GameFlow> stateMachine { get; private set; }

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        public string currentState
        {
            get
            {
                if (!Application.isPlaying)
                {
                    return null;
                }

                return stateMachine.currentState.GetType().Name;
            }
        }

        public void OnInit()
        {
            stateMachine = new StateMachine<GameFlow>(this);
            stateMachine.Add<EntryState>();
            stateMachine.Add<HomeState>();
            stateMachine.Add<GameState>();
        }

        public void Run()
        {
            stateMachine.Run<EntryState>();
        }


        public void OnUpdate(float deltaTime)
        {
            stateMachine.OnUpdate();
        }

        public void Dispose()
        {
        }
    }
}