using UnityToolkit;

namespace GGJ2026.GamePlay
{
    public class GamingState : IState<GameMgr>
    {
        public void OnInit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public void OnEnter(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            GamingMgr.Singleton.StartGame();
        }

        public void Transition(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            if (GamingMgr.Singleton.isGameOver)
            {
                stateMachine.Run<GameEndState>();
            }
        }


        public void OnUpdate(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
        }

        public void OnExit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            GamingMgr.Singleton.EndGame();
        }
    }
}