using System;
using cfg;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    [Serializable]
    public class GameData : IModel
    {
        public static string defaultDataFileName => "GameData";

        public GameLevelEnum lastCompletedLevel = GameLevelEnum.None;
    }
}