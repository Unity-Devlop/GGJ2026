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

        public PlayerData playerData;
    }
}