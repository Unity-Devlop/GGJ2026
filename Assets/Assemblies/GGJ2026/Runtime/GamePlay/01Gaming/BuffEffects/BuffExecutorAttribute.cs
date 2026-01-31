using cfg;

namespace GGJ2026.GamePlay
{
    public class BuffExecutorAttribute : System.Attribute
    {
        public readonly BuffEnum buffEnum;

        public BuffExecutorAttribute(BuffEnum buffEnum)
        {
            this.buffEnum = buffEnum;
        }
    }
}