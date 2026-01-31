using System;
using System.Collections.Generic;
using cfg;

namespace GGJ2026.GamePlay
{
    [Serializable]
    public class EnemyData
    {
        public GhostEnum id;
        public EntityPropertyData property;
        public List<CardData> candidateCards;
    }
}