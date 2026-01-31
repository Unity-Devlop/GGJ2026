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
    }
}