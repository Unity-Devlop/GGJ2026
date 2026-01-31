using cfg;

namespace GGJ2026.GamePlay
{
    public struct SwitchMaskOperation : IOperation
    {
        public MaskEnum id;
        public bool endRoundRightAfter;

        public SwitchMaskOperation(MaskEnum id, bool endRoundRightAfter)
        {
            this.id = id;
            this.endRoundRightAfter = endRoundRightAfter;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + id.GetHashCode();
            hash = hash * 31 + endRoundRightAfter.GetHashCode();
            return hash;
        }
    }
}