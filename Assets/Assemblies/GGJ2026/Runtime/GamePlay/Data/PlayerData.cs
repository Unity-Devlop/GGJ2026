using System;
using System.Collections.Generic;
using UnityToolkit;

namespace GGJ2026
{
    [Serializable]
    public class PlayerData : IModel
    {
        public List<CardData> cards = new();
    }
}