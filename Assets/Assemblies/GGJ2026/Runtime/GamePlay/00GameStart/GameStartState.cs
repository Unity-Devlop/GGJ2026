using cfg;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    public class GameStartState : IState<GameMgr>
    {
        public void OnInit(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public void OnEnter(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            Global.localSave.Get<GameData>(out var data);
            var lastCompletedLevel = data.lastCompletedLevel;

            // 如果已经打通最后一关了 就从头开始
            if (EnumHelper<GameLevelEnum>.keys[^1] == lastCompletedLevel)
            {
                lastCompletedLevel = GameLevelEnum.第一关;
            }
            else if (lastCompletedLevel == GameLevelEnum.None)
            {
                lastCompletedLevel = GameLevelEnum.第一关;
            }

            data.lastCompletedLevel = lastCompletedLevel;
        }

        public void Transition(GameMgr owner, IStateMachine<GameMgr> stateMachine)
        {
            stateMachine.Change<GamingState>();
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