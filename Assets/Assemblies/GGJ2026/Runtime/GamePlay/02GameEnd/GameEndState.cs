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
            if (GamingMgr.Singleton.isGameWin)
            {
                UIRoot.Singleton.OpenPanel<CompleteLevelPanel>();
            }
            else
            {
                UIRoot.Singleton.OpenPanel<FailedLevelPanel>();
            }
        }

        public void Transition(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            if (UIRoot.Singleton.GetOpenedPanel(out CompleteLevelPanel panel))
            {
                if (panel.IsConfirmed)
                {
                    UIRoot.Singleton.Dispose<CompleteLevelPanel>();
                    UIRoot.Singleton.Dispose<FailedLevelPanel>();
                    stateMachine.Change<GameStartState>();
                }
            }
            else if (UIRoot.Singleton.GetOpenedPanel(out FailedLevelPanel failedLevelPanel))
            {
                if (failedLevelPanel.isConfirmed)
                {
                    UIRoot.Singleton.Dispose<CompleteLevelPanel>();
                    UIRoot.Singleton.Dispose<FailedLevelPanel>();
                    Global.gameFlow.stateMachine.Change<HomeState>();
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
            UIRoot.Singleton.Dispose<FailedLevelPanel>();
        }
    }
}