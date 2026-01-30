using cfg;
using Cysharp.Threading.Tasks;
using Jump.GamePlay;
using UnityEngine.AddressableAssets;
using UnityToolkit;

namespace Jump
{
    public class GameState : IState<GameFlow>
    {
        private bool loading;

        public void OnInit(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public async void OnEnter(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            if (loading) return;
            loading = true;
            GlobalLogger.LogEditor("进入游戏场景");
            Global.localSave.Get<GameData>(out var data);

            if (data.lastPlayedMap == GameMapEnum.None)
            {
                data.lastPlayedMap = GameMapEnum.Started;
                data.lastPlayedLevelIndex = -1;
            }

            // TODO 根据游戏模式切换不同场景
            await Addressables.LoadSceneAsync(Global.refHolder.gameScene);
            await UIRoot.Singleton.OpenPanelAsync<GamePlayPanel>();
            loading = false;
        }

        public void Transition(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public void OnUpdate(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public void OnExit(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            UIRoot.Singleton.ClosePanel<GamePlayPanel>();
        }
    }
}