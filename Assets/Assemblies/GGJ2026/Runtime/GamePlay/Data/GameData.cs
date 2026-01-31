using System;
using System.Collections.Generic;
using cfg;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    [Serializable]
    public class GameData : IModel
    {
        public static string defaultDataFileName => "GameData";

        public GameLevelEnum lastCompletedLevel = GameLevelEnum.None;

        public SerializableDictionary<GameLevelEnum, PlayerData> levelPlayerData = new();
        public SerializableDictionary<GameLevelEnum, EnemyData> levelEnemyData = new();
    }
}