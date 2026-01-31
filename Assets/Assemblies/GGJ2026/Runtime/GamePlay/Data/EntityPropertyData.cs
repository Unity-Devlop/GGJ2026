using System;
using UnityToolkit;

namespace GGJ2026.GamePlay
{
    [Serializable]
    public class EntityPropertyData
    {
        public Property<int> health = new(1);
        public int shield = 0;
    }
}