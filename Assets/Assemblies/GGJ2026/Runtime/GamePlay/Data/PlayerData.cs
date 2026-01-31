using System;
using System.Collections.Generic;
using cfg;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    [Serializable]
    public class PlayerData : IModel
    {
        public List<CardData> cards = new();
        public EntityPropertyData property = new();
        public MaskEnum currentMask = MaskEnum.本我;
        public bool randomDrawCard = false;
        public List<CardEnum> candidateCards = new();
    }
}