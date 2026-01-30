using System;
using DebugUI;
using Jump.GamePlay;
using UnityEngine.UIElements;

namespace Jump.Home
{
    public class HomeDebugger : DebugUIBuilderBase
    {
        protected override void Configure(IDebugUIBuilder builder)
        {
            foreach (var gameMapConfig in Global.tables.GameMapTable.DataList)
            {
                var mapConfig = gameMapConfig;
                builder.AddButton($"Load Map: {mapConfig.Id}", () =>
                {
                    Global.localSave.Get<GameData>(out var gameData);
                    gameData.lastPlayedMap = mapConfig.Id;
                    Global.gameFlow.stateMachine.Change<GameState>();
                });
            }
        }

        private void OnEnable()
        {
            GetComponent<UIDocument>().enabled = true;
        }

        private void OnDisable()
        {
            GetComponent<UIDocument>().enabled = false;
        }
    }
}