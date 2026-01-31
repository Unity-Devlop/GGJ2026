namespace GGJ2026.GamePlay
{
    public struct UseCardOperation : IOperation
    {
        public CardData cardData;
        public UseCardOperation(CardData cardData)
        {
            this.cardData = cardData;
        }
    }
}