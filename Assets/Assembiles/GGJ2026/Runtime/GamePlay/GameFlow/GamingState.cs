using UnityToolkit;

namespace Jump.GamePlay
{
    public class GamingState : IState<GameMgr>
    {
        public void OnInit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public async void OnEnter(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
        }

        public void Transition(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
        }


        public void OnUpdate(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public void OnExit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            // throw new System.NotImplementedException();
        }
    }
}