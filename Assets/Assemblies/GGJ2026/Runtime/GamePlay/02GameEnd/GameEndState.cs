using UnityToolkit;

namespace GGJ2026.GamePlay
{
    public class GameEndState : IState<GameMgr>
    {
        public void OnInit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public void OnEnter(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            // throw new System.NotImplementedException();
            UIRoot.Singleton.OpenPanel<CompleteLevelPanel>();
        }

        public void Transition(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            if(UIRoot.Singleton.GetOpenedPanel(out CompleteLevelPanel panel))
            {
                if (panel.IsConfirmed)
                {
                    stateMachine.Change<GameStartState>();
                }
            }
        }

        public void OnUpdate(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public void OnExit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            UIRoot.Singleton.Dispose<CompleteLevelPanel>();
        }
    }
}