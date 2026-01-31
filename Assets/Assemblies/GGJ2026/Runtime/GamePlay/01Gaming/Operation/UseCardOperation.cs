namespace GGJ2026.GamePlay
{
    public struct UseCardOperation : IOperation
    {
        public CardData cardData;
        public UseCardOperation(CardData cardData)
        {
            this.cardData = cardData;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + (cardData != null ? cardData.GetHashCode() : 0);
            return hash;
        }

        public override string ToString()
        {
            return $"UseCardOperation: {cardData.config.Id}";
        }
    }
}