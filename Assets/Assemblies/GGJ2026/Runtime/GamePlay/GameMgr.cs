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
        public StateMachine<GameMgr> stateMachine { get; private set; }


        [field: SerializeField] public CinemachineCamera main { get; private set; }

        protected async override void OnInit()
        {
            Global.localSave.Get<GameData>(out var data);
            Global.cameraSystem.cinemachineBrain.enabled = true;
            // TODO 绑定UI等等操作

            stateMachine = new StateMachine<GameMgr>(this);

            stateMachine.Add<GameStartState>();
            stateMachine.Add<GamingState>();
            stateMachine.Add<GameEndState>();

            stateMachine.Run<GameStartState>();


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