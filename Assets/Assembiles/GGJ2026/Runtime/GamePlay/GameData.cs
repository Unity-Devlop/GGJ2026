using System;
using System.Collections.Generic;
using cfg;
using UnityToolkit;

namespace Jump.GamePlay
{
    [Serializable]
    public class GameData : IModel
    {
        /// <summary>
        /// 已经解锁的关卡地图
        /// </summary>
        public List<GameMapEnum> unlockedMaps = new List<GameMapEnum>();

        /// <summary>
        /// 所有的关卡地图
        /// </summary>
        public List<GameMapEnum> totalMaps = new List<GameMapEnum>();


        public GameMapEnum lastPlayedMap = GameMapEnum.None;
        public int lastPlayedLevelIndex = 0;
        public static string defaultDataFileName => "GameData.json";
    }
}