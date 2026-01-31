using System;
using cfg;
using UnityEngine;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "GGJ2026/LevelConfig", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        public SerializableDictionary<GameLevelEnum, PlayerData> levelPlayerData = new();
        public SerializableDictionary<GameLevelEnum, EnemyData> levelEnemyData = new();
    }
}