using cfg;
using GGJ2026.GamePlay;
using UnityToolkit;

namespace GGJ2026
{
    public class EntryState : IState<GameFlow>
    {
        public void OnInit(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
        }

        public void OnEnter(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            bool haveData = LocalSaveSystem.Read(GameData.defaultDataFileName, out GameData gameData);
            if (!haveData)
            {
                GlobalLogger.LogEditor("没有找到本地存档，创建默认存档");
                gameData = new GameData();

                foreach (var gameMapConfig in Global.tables.GameMapTable.DataList)
                {
#if DEVELOPMENT
                    if (gameMapConfig.Id == GameMapEnum.Developer)
                    {
                        continue;
                    }   
#endif
                    gameData.totalMaps.Add(gameMapConfig.Id);
                }

                gameData.unlockedMaps.Add(GameMapEnum.Started);
#if DEVELOPMENT
                gameData.unlockedMaps.Add(GameMapEnum.Developer);
#endif
                gameData.lastPlayedMap = GameMapEnum.None; // 表示没有玩过任何地图
                gameData.lastPlayedLevelIndex = -1; // 表示没有玩过任何关卡

                LocalSaveSystem.Write(GameData.defaultDataFileName, gameData);
            }

            Global.localSave.Add(gameData);
        }

        public void Transition(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            // TODO 第一次玩游戏 直接让玩家玩第一关 而不是走首页
            stateMachine.Change<HomeState>();
        }

        public void OnUpdate(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            // throw new System.NotImplementedException();
        }

        public void OnExit(GameFlow owner, IStateMachine<GameFlow> stateMachine)
        {
            // throw new System.NotImplementedException();
        }
    }
}