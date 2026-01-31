using System;
using System.Collections.Generic;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    [Serializable]
    public class PlayerData : IModel
    {
        public List<CardData> cards = new();
        public EntityPropertyData property = new();
    }
}