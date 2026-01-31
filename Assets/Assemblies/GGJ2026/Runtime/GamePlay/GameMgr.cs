using System;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    public class GameMgr : MonoSingleton<GameMgr>
    {
        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        public StateMachine<GameMgr> stateMachine { get; private set; }

        [Sirenix.OdinInspector.ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        public IState<GameMgr> currentState => stateMachine.currentState;


        [field: SerializeField] public CinemachineCamera main { get; private set; }

        protected async override void OnInit()
        {
            Global.localSave.Get<GameData>(out var data);
            Global.cameraSystem.cinemachineBrain.enabled = true;
            // TODO 绑定UI等等操作

            stateMachine = new StateMachine<GameMgr>(this);
            stateMachine.OnStateChange += (oldState, newState) =>
            {
                Debug.Log($"GameMgr State Change: {oldState?.GetType().Name} -> {newState.GetType().Name}");
            };

            stateMachine.Add<GameStartState>();
            stateMachine.Add<GamingState>();
            stateMachine.Add<GameEndState>();

            stateMachine.Run<GameStartState>();
        }

        // private float tickRate = 1f / 30f;
        // private float timer = 0f;

        private void Update()
        {
            // timer += Time.deltaTime;
            // if (timer < tickRate)
            // {
            //     return;
            // }
            //
            // timer -= tickRate;

            stateMachine.OnUpdate();
        }


        protected override void OnDispose()
        {
            if (UIRoot.SingletonNullable != null)
            {
                UIRoot.Singleton.Dispose<GamePlayPanel>();
            }
        }
    }
}