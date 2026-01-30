using Jump.Home;
using UnityEngine.AddressableAssets;
using UnityToolkit;

namespace Jump
{
    public class HomeState : IState<GameFlow>
    {
        public void OnInit(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public void OnEnter(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            Addressables.LoadSceneAsync(Global.refHolder.homeScene).WaitForCompletion();
            UIRoot.Singleton.OpenPanel<HomePanel>();
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
            UIRoot.Singleton.Dispose<HomePanel>();
        }
    }
}