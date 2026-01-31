using System;
using System.Collections.Generic;

namespace GGJ2026.GamePlay
{
    [Serializable]
    public class EnemyData 
    {
        public EntityPropertyData property;
        public List<CardData> candidateCards;
    }
}