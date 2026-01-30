using System;
using DebugUI;
using GGJ2026.GamePlay;
using UnityEngine.UIElements;

namespace GGJ2026.Home
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