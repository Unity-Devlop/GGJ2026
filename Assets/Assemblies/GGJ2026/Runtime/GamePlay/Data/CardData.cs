using System;
using cfg;

namespace GGJ2026.GamePlay
{
    [Serializable]
    public class CardData
    {
        public CardEnum id;
        public CardConfig config => Global.tables.CardTable.Get(id);

        public CardData(CardEnum id)
        {
            this.id = id;
        }
    }
}