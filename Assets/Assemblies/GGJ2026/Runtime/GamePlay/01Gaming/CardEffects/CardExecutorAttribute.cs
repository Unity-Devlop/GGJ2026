using cfg;

namespace GGJ2026.GamePlay
{
    public class CardExecutorAttribute : System.Attribute
    {
        public CardEnum cardEnum;

        public CardExecutorAttribute(CardEnum cardEnum)
        {
            this.cardEnum = cardEnum;
        }
    }
}