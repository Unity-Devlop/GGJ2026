using System;
using System.Collections.Generic;

namespace GGJ2026.GamePlay
{
    [Serializable]
    public class EnemyData 
    {
        public EntityPropertyData propertyData;
        public List<CardData> candidateCards;
    }
}